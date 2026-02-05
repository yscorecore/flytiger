using Microsoft.CodeAnalysis;

namespace FlyTiger.Mapper.Generators
{
    internal abstract class BaseGenerator
    {
        public abstract void AppendFunctions(MapperContext context);


        protected void ReportTargetIsValueTypeCanNotCopy(MapperContext context)
        {

        }

        protected void ReportTargetPrpertyNotFilled(MapperContext context, IPropertySymbol targetProperty)
        {
            if (context.MappingInfo.CheckTargetPropertiesFullFilled)
            {
                var targetPropertyLocation = LocationUtils.FindMemberLocation(targetProperty);
                var location = targetPropertyLocation == Location.None ? LocationUtils.FindAttributeLocation(context.MappingInfo.FromAttribute) : targetPropertyLocation;
                var propertyName = targetProperty.Name;
                var fromType = context.MappingInfo.SourceType.ToErrorFullString();
                var toType = context.MappingInfo.TargetType.ToErrorFullString();
                context.CodeWriter.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.TargetPropertyNotFilled, location, propertyName, fromType, toType));
            }
        }
        protected void ReportSourcePropertyNotMapped(MapperContext context, IPropertySymbol sourceProperty)
        {
            if (context.MappingInfo.CheckSourcePropertiesFullUsed)
            {
                var sorucePropertyLocation = LocationUtils.FindMemberLocation(sourceProperty);
                var location = sorucePropertyLocation == Location.None ? LocationUtils.FindAttributeLocation(context.MappingInfo.FromAttribute) : sorucePropertyLocation;
                var propertyName = sourceProperty.Name;
                var fromType = context.MappingInfo.SourceType.ToErrorFullString();
                var toType = context.MappingInfo.TargetType.ToErrorFullString();
                context.CodeWriter.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.SorucePropertyNotMapped, location, propertyName, fromType, toType));
            }
        }
        protected void ReportPropertyCanNotBeMapped(MapperContext context, IPropertySymbol targetProperty, IPropertySymbol sourceProperty)
        {
            var targetPropertyLocation = LocationUtils.FindMemberLocation(targetProperty);
            var location = targetPropertyLocation == Location.None ? LocationUtils.FindAttributeLocation(context.MappingInfo.FromAttribute) : targetPropertyLocation;
            var propertyName = targetProperty.Name;
            var fromType = context.MappingInfo.SourceType.ToErrorFullString();
            var toType = context.MappingInfo.TargetType.ToErrorFullString();
            context.CodeWriter.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CanNotMapProperty, location, targetProperty.Name, fromType, toType));
        }

    }
}
