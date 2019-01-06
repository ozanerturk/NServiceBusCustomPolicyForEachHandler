using NServiceBus;

namespace WebApplication1.CustomPolicy
{
    public interface ICustomRecoveryPolicyFactory
    {
        void AddPolicy<T>(int immediateRetryCount, int delayedRetryCount, int delayedRetryTimeIncrease) where T : IMessage;
        CustomRecoveryPolicy GetPolicy(string messageName);
    }
}