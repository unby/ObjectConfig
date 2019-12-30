using ObjectConfig.Data;
using System;
using System.Collections.Generic;

namespace ObjectConfig
{
    public class ObjectMaker
    {
        Dictionary<Type, Func<object>> DataTypeDictionary = new Dictionary<Type, Func<object>>
    {
 
        };

        public static ValueElement CreateValueElement(Action<ValueElement> enrichmentAction)
        {
            var result = new ValueElement();// { ValueElementId = Utils.NewSequentialId };
            enrichmentAction?.Invoke(result);
            return result;
        }

        public static TypeElement CreateTypeElement(Action<TypeElement> enrichmentAction)
        {
            var result = new TypeElement();// { TypeElementId = Utils.NewSequentialId };
            enrichmentAction?.Invoke(result);
            return result;
        }

        public static Application CreateApplication(Action<Application> enrichmentAction)
        {
            var result = new Application();// { ApplicationId = Utils.NewSequentialId };
            enrichmentAction?.Invoke(result);
            return result;
        }

        public static Data.Environment CreateEnvironment(Action<Data.Environment> enrichmentAction)
        {
            var result = new Data.Environment();// { ApplicationId = Utils.NewSequentialId };
            enrichmentAction?.Invoke(result);
            return result;
        }
        public static Config CreateConfig(Action<Config> enrichmentAction)
        {
            var result = new Config();// { ConfigId = Utils.NewSequentialId };
            enrichmentAction?.Invoke(result);
            return result;
        }
    }
}
