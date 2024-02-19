using Microsoft.CodeAnalysis;

namespace FlyTiger.Mapper.Generators
{
    internal abstract class BaseGenerator
    {
        public abstract void AppendFunctions(MapperContext context);
       

        protected void ReportTargetIsValueTypeCanNotCopy(MapperContext context)
        {

        }
        protected void ReportInitOnlyPropertyCanNotCopyValue(MapperContext context, IPropertySymbol targetProperty, IPropertySymbol sourceProperty)
        {

        }
        protected void ReportTargetPrpertyNotFilled(MapperContext context, IPropertySymbol targetProperty)
        {
            if (context.MappingInfo.CheckTargetPropertiesFullFilled)
            {
                context.CodeWriter.Context.ReportTargetPropertyNotFilled(targetProperty, context.MappingInfo.SourceType, context.MappingInfo.TargetType);
            }
        }
        protected void ReportSourcePropertyNotMapped(MapperContext context, IPropertySymbol sourceProperty)
        {
            if (context.MappingInfo.CheckSourcePropertiesFullUsed)
            {
                context.CodeWriter.Context.ReportSourcePropertyNotMapped(sourceProperty, context.MappingInfo.SourceType, context.MappingInfo.TargetType);
            }
        }
        protected void ReportPropertyCanNotBeMapped(MapperContext context, IPropertySymbol targetProperty, IPropertySymbol sourceProperty)
        {

        }
        protected void ReportReadOnlyPropertyCanNotFilled(MapperContext context, IPropertySymbol targetProperty, IPropertySymbol sourceProperty)
        {
            var mappingInfo = context.MappingInfo;
            context.CodeWriter.Context.ReportTargetPropertyNotFilledBecauseIsGetOnly(targetProperty, mappingInfo.SourceType, mappingInfo.TargetType);

        }
    }
}
