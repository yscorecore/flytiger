using System;
using System.Collections.Generic;
using System.Text;

namespace FlyTiger
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class AutoConstructorIgnoreAttribute : Attribute
    {
    }
}
