using HKA.MediatRExtras.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace HKA.MediatRExtras.DataResolvers;

internal class SensitiveDataResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (member is PropertyInfo prop)
        {
            if (Attribute.IsDefined(prop, typeof(OperationLogSensitiveDataAttribute)))
            {
                property.ValueProvider = new SensitiveStringValueProvider();
            }
        }
        return property;
    }

    internal class SensitiveStringValueProvider : IValueProvider
    {
        public const string SensitiveDataString = "**SensitiveData**";
        public void SetValue(object target, object? value)
        {
            throw new NotSupportedException();
        }

        public object? GetValue(object target)
        {
            return SensitiveDataString;
        }
    }
}
