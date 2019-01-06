using NServiceBus;
using NServiceBus.Transport;
using System;

namespace WebApplication1.CustomPolicy.Extensions
{
    public static class CustomRecoverablilityExtensions
    {
        public static Func<RecoverabilityConfig, ErrorContext, RecoverabilityAction> GetRecoverabilityAction(this ICustomRecoveryPolicyFactory policyFactory)
        {
            return (config, context) =>
            {
                var policyKey = context.Message.Headers["NServiceBus.EnclosedMessageTypes"].Split(",")[0];//this gives us the full name of particular message type.
                var policy = policyFactory.GetPolicy(policyKey);
                var action = DefaultRecoverabilityPolicy.Invoke(config, context);

                #region Check For UnrecoverableExceptionTypes
                foreach (var exceptionType in config.Failed.UnrecoverableExceptionTypes)
                {
                    if (exceptionType.IsInstanceOfType(context.Exception))
                    {
                        return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
                    }
                }
                #endregion

                if (policy != null)
                {
                    if (action is ImmediateRetry immediateRetry)
                    {
                        if (context.ImmediateProcessingFailures <= policy.ImmediateRetryCount)
                        {
                            return RecoverabilityAction.ImmediateRetry();
                        }
                        else if (context.DelayedDeliveriesPerformed <= policy.DelayedRetryCount)
                        {
                            return RecoverabilityAction.DelayedRetry(TimeSpan.FromSeconds(policy.DelatedRetryTimeIncrease));
                        }
                        else return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
                    }
                    else if (action is DelayedRetry act && context.DelayedDeliveriesPerformed <= policy.DelayedRetryCount)
                    {
                        return RecoverabilityAction.DelayedRetry(TimeSpan.FromSeconds(policy.DelatedRetryTimeIncrease));
                    }
                    else
                    {
                        return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
                    }
                }

                return action;
            };
        }

    }
}
