namespace DbLite
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AutoIncrementAttribute : Attribute
    {
    }
}
