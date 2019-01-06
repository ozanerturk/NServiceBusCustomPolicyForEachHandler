# NServiceBusCustomPolicyForEachHandler
Define recoverability policy for each handler.

NServiceBus does not provide a feature that you can manage Recoverability Policy for each handler.

Register CustomRecoveryPolicyFactory to container: 

```csharp
//Startup.cs
var policyFactory = new CustomRecoveryPolicyFactory();
services.AddSingleton<ICustomRecoveryPolicyFactory>(policyFactory);
```

And just inject the factory to Handler constructor.
    
```csharp
//WithCustomPolicyHandler.cs 
public class WithCustomPolicyHandler : IHandleMessages<WithCustomPolicyMessage>
  {
      public WithCustomPolicyHandler(ICustomRecoveryPolicyFactory factory)
      {
          factory.AddPolicy<WithCustomPolicyMessage>(immediateRetryCount: 1, delayedRetryCount: 2, delayedRetryTimeIncrease: 5);
      }
      public Task Handle(WithCustomPolicyMessage message, IMessageHandlerContext context)
      {
          throw new Exception();
      }
  }
  ```

`AddPolicy<T>` method creates a `CustomRecoveryPolicy` and adds it' dictionary.

```csharp
//CustomRecoveryPolicyFactory.cs
 public void AddPolicy<T>(int immediateRetryCount, int delayedRetryCount, int delayedRetryTimeIncrease) where T : IMessage
        {
            var key = typeof(T).FullName; //Namespace.WithCustomPolicyMessage
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
```

`CustomRecoverablilityExtensions`has an extension method which is extended for policyFactory instance.
we need this instance to resolve our custom policy.

to find particular policy, this method gets type name from message header.

```csharp
//CustomRecoverablilityExtensions.cs
var policyKey = context.Message.Headers["NServiceBus.EnclosedMessageTypes"].Split(",")[0];
```
this method returns a `Func` which meets the CustomPolicy to NServiceBus

```csharp
//Startup.cs
recoverability.CustomPolicy(policyFactory.GetRecoverabilityAction());
```

 
