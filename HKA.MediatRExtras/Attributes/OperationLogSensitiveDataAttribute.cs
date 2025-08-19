namespace HKA.MediatRExtras.Attributes;

/// <summary>
/// A marker attribute for sensitive data
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
public class OperationLogSensitiveDataAttribute : Attribute
{

}