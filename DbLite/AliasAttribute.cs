namespace DbLite
{
    using System;

    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class AliasAttribute : Attribute
    {
        readonly string name;
        public string Name
        {
            get { return name; }
        }

        public AliasAttribute(string name)
        {
            this.name = name;
        }
    }
}
