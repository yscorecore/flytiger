﻿using System;

namespace FlyTiger
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class SingletonPatternAttribute : Attribute
    {
        internal const string DefaultInstanceName = "Instance";
        public string InstancePropertyName { get; set; } = DefaultInstanceName;
    }
}