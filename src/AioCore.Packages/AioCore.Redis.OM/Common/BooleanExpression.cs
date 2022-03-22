using System.Linq.Expressions;

namespace AioCore.Redis.OM.Common
{
    public abstract class BooleanExpression
    {
        internal BooleanExpression(LambdaExpression expression)
        {
            Expression = expression;
        }

        protected LambdaExpression Expression { get; set; }

        protected abstract void ValidateAndPushOperand(Expression expression, Stack<string> stack);

        protected abstract void SplitBinaryExpression(BinaryExpression expression, Stack<string> stack);

        protected Stack<string> SplitExpression()
        {
            var ret = new Stack<string>();
            if (Expression.Body is BinaryExpression binExpression)
            {
                SplitBinaryExpression(binExpression, ret);
            }
            else
            {
                ValidateAndPushOperand(Expression.Body, ret);
            }

            return ret;
        }
    }
}