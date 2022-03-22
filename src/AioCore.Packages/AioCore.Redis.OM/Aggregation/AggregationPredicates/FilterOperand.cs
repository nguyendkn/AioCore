namespace AioCore.Redis.OM.Aggregation.AggregationPredicates
{
    internal class FilterOperand
    {
        private readonly string _text;
        private readonly FilterOperandType _operandType;


        internal FilterOperand(string text, FilterOperandType operandType)
        {
            _text = text;
            _operandType = operandType;
        }


        public override string ToString()
        {
            return _operandType switch
            {
                FilterOperandType.Identifier => $"@{_text}",
                FilterOperandType.Numeric => _text,
                FilterOperandType.String => $"'{_text}'",
                _ => string.Empty
            };
        }
    }
}