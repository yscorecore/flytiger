using System;

namespace FlyTiger.Mapper
{
    partial class MapperGenerator
    {
        internal const string AttributeShortName = "Mapper";
        internal const string AttributeName = "MapperAttribute";
        internal const string IgnorePropertiesPropertyName = "IgnoreProperties";
        internal const string CustomMappingsPropertyName = "CustomMappings";
        internal const string CheckTypeName = "CheckType";
        internal const string MapperTypeName = "MapperType";

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

        public string[] IgnoreProperties { get; set; }

        public string[] CustomMappings { get; set; }

        public MapperType MapperType { get; set; } = MapperType.All;
        
        public CheckType CheckType { get; set; } = CheckType.None;

    }
    [Flags]
    enum MapperType
    {
        Query = 1,
        Convert = 2,
        Update = 4,
        BatchUpdate = Convert | Update,
        All = Query | Convert | Update
    }
    [Flags]
    enum CheckType
    {
        None = 0,
        SourceMembersFullUsed = 1,
        TargetMembersFullFilled = 2,
        All = SourceMembersFullUsed | TargetMembersFullFilled
    }
    enum CollectionUpdateMode
    {
        Append = 0,
        Merge = 1,
        Update = 2
    }
}
";
    }
}
