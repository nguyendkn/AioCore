namespace AioCore.Shared.ValueObjects;

public class QueryAdvanced
{
    public string Field { get; set; }
    public Function Function { get; set; }
    public AttributeType ValueType { get; set; }
    public Operator Operator { get; set; }
    public string Value { get; set; }

    public QueryAdvanced(
        string field,
        Function function,
        AttributeType valueType,
        string value,
        Operator @operator = Operator.And)
    {
        Field = field;
        Function = function;
        ValueType = valueType;
        Value = value;
        Operator = @operator;
    }
}

public enum Function
{
    Equal,
    NotEqual,
    Between,
    In,
    NotIn
}

public enum Operator
{
    And = 1,
    Or = 2,
    Not = 3
}