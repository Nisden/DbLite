namespace DbLite
{
    using System;

    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public sealed class IgnoreAttribute : Attribute
    {
    }
}
