using System;
using System.Collections.Generic;
using System.Text;

namespace FlyTiger
{
    partial class MapperGenerator
    {
        const string AttributeName = "MapperAttribute";

        internal const string IgnoreTargetPropertiesPropertyName = "IgnoreTargetProperties";
        internal const string CustomMappingsPropertyName = "CustomMappings";

        internal static string AttributeFullName = $"{NameSpaceName}.{AttributeName}";
        const string AttributeCode = @"using System;
namespace FlyTiger
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class MapperAttribute : Attribute
    {
        public MapperAttribute(Type sourceType, Type targetType)
        {

            SourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
        }

        public Type SourceType { get; }
        public Type TargetType { get; }

        public string[] IgnoreTargetProperties { get; set; }

        public string[] CustomMappings { get; set; }

    }
}
";
    }
}
