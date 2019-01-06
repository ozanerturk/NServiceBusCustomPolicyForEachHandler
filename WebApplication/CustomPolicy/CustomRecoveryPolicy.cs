namespace WebApplication1.CustomPolicy
{
    public class CustomRecoveryPolicy
    {
        public int DelayedRetryCount { get; internal set; }
        public int ImmediateRetryCount { get; internal set; }
        public int DelatedRetryTimeIncrease { get; internal set; }
    }
}