using MiniFrameWork.Camadas;
using Newtonsoft.Json.Serialization;

namespace MiniFrameWork.Util
{
    public class ShouldSerializeContractResolver<E>  : DefaultContractResolver   where E: EntityBase
    {

        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (property.DeclaringType == typeof(EntityBase) || property.DeclaringType.BaseType == typeof(EntityBase))
            {
                if (property.PropertyName == "serializableProperties")
                {
                    property.ShouldSerialize = instance => { return false; };
                }
                else
                {
                    property.ShouldSerialize = instance =>
                    {
                        E p = (E)instance;
                        return (p).serializableProperties.Contains(property.PropertyName);
                    };
                }
            }
            return property;
        }
    }

}