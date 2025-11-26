using Microsoft.EntityFrameworkCore.Metadata;

public sealed class InsertExpression : Expression
{
    public string Table { get; }
    public string? Schema { get; }
    public IReadOnlyList<IProperty> Properties { get; }
    public SelectExpression Source { get; }
    public SqlExpression Projection { get; }

    public InsertExpression(
        string table,
        string? schema,
        IReadOnlyList<IProperty> properties,
        SelectExpression source,
        SqlExpression projection)
    {
        Table = table;
        Schema = schema;
        Properties = properties;
        Source = source;
        Projection = projection;
    }

    public override ExpressionType NodeType => ExpressionType.Extension;
    public override Type Type => typeof(int);
}
