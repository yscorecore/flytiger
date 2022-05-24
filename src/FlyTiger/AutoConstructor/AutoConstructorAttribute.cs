using System;
using System.Collections.Generic;
using System.Text;

namespace FlyTiger
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class AutoConstructorAttribute : Attribute
    {
        public bool NullCheck { get; set; } = false;
    }
}
