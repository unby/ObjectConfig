using Newtonsoft.Json;

namespace UnitTests
{
    public class ContractResolverWithPrivates : Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver
    {
        protected override Newtonsoft.Json.Serialization.JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
        {
            Newtonsoft.Json.Serialization.JsonProperty prop = base.CreateProperty(member, memberSerialization);

            if (!prop.Writable)
            {
                System.Reflection.PropertyInfo property = member as System.Reflection.PropertyInfo;
                if (property != null)
                {
                    bool hasPrivateSetter = property.GetSetMethod(true) != null;
                    prop.Writable = hasPrivateSetter;
                }
            }

            return prop;
        }
    }
}
