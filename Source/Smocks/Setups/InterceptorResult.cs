namespace Smocks.Setups
{
    public class InterceptorResult
    {
        public bool Intercepted { get; set; }

        public InterceptorResult(bool intercepted)
        {
            Intercepted = intercepted;
        }
    }
}