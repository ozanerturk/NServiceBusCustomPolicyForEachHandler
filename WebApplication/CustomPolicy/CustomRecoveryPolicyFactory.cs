using NServiceBus;
using System.Collections.Generic;

namespace WebApplication1.CustomPolicy
{
    public class CustomRecoveryPolicyFactory : ICustomRecoveryPolicyFactory
    {
        private Dictionary<string, CustomRecoveryPolicy> _policyDictionary;

        public CustomRecoveryPolicyFactory()
        {
            _policyDictionary = new Dictionary<string, CustomRecoveryPolicy>();
        }

        public void AddPolicy<T>(int immediateRetryCount, int delayedRetryCount, int delayedRetryTimeIncrease) where T : IMessage
        {
            var key = typeof(T).FullName;
            if (!_policyDictionary.ContainsKey(key))
            {
                var policy = new CustomRecoveryPolicy()
                {
                    ImmediateRetryCount = immediateRetryCount,
                    DelayedRetryCount = delayedRetryCount,
                    DelatedRetryTimeIncrease = delayedRetryTimeIncrease
                };
                _policyDictionary.Add(key, policy);
            }
        }

        public CustomRecoveryPolicy GetPolicy(string messageName)
        {
            return _policyDictionary.ContainsKey(messageName) ? _policyDictionary[messageName] : null;
        }
    }
}