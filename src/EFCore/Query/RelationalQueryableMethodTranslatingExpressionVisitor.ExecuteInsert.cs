using Microsoft.EntityFrameworkCore.Query;

public partial class RelationalQueryableMethodTranslatingExpressionVisitor
{
    protected virtual ShapedQueryExpression? TranslateExecuteInsert(
        ShapedQueryExpression source,
        LambdaExpression selector)
    {
        var selectExpression = source.QueryExpression as SelectExpression;
        if (selectExpression == null)
        {
            return null;
        }

        // ✅ استخراج جدول مقصد از Type پروجکشن
        var targetType = selector.ReturnType;
        var targetEntityType = _model.FindEntityType(targetType);
        if (targetEntityType == null)
        {
            throw new InvalidOperationException(
                RelationalStrings.ExecuteInsertTargetNotMapped(targetType.ShortDisplayName()));
        }

        var targetTable = targetEntityType.GetTableName();
        var targetSchema = targetEntityType.GetSchema();

        // ✅ ساختن projection سمت SELECT
        var projection = TranslateLambdaExpression(
            source,
            selector);

        if (projection == null)
        {
            return null;
        }

        // ✅ ساخت Expression مخصوص Insert
        var insertExpression = new InsertExpression(
            targetTable!,
            targetSchema,
            targetEntityType.GetProperties(),
            selectExpression,
            projection);

        // ✅ تولید Command نهایی
        var newSelectExpression = _sqlExpressionFactory.Select(insertExpression);

        return new ShapedQueryExpression(
            newSelectExpression,
            source.ShaperExpression);
    }
}
