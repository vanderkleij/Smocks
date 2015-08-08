namespace Smocks.Setups
{
    public class InterceptorResult<TReturnValue> : InterceptorResult
    {
        public InterceptorResult(bool intercepted, TReturnValue returnValue)
            : base(intercepted)
        {
            ReturnValue = returnValue;
        }

        public TReturnValue ReturnValue { get; set; }
    }
}