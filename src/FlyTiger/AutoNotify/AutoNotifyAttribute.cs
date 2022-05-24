using System;
// ReSharper disable once CheckNamespace
namespace FlyTiger
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class AutoNotifyAttribute : Attribute
    {
        public string PropertyName { get; set; }
    }
}
