using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace FlyTiger.Mapper.Generators
{
    internal class GenericFunctionGenerator
    {
        public void AppendFunctions(CodeWriter codeWriter,
           CsharpCodeBuilder codeBuilder, IList<AttributeData> attributeDatas)
        {
            var allSources = attributeDatas.Where(p => p.AttributeClass.Is(MapperGenerator.AttributeFullName))
                .Select(p => ConvertMappingInfo.FromAttributeData(p))
                .ToLookup(p => (p.SourceType, p.SourceTypeFullDisplay));
            foreach (var item in allSources)
            {
                AppendGenericFunctions(item.Key, item.ToList(), codeBuilder);
            }
        }
        private void AppendGenericFunctions((ITypeSymbol Type, string Display) fromType, List<ConvertMappingInfo> mappingInfos,
            CsharpCodeBuilder codeBuilder)
        {
            var methodName = "To";
            AddToMethodForSingle();
            AddToMethodForSingleWithPostAction();
            AddCopyToMethodForSingle();
            AddCopyToMethodForCollection();
            AddToMethodForEnumable();
            AddToMethodForEnumableWithPostAction();
            AddToMethodForQueryable();

            void AddToMethodForSingle()
            {
                codeBuilder.AppendCodeLines(
                    $"public static T {methodName}<T>(this {fromType.Display} source) where T:new()");
                codeBuilder.BeginSegment();
                if (!fromType.Type.IsValueType)
                {
                    codeBuilder.AppendCodeLines("if (source == null) return default;");
                }

                foreach (var mapping in mappingInfos)
                {
                    codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({mapping.TargetTypeFullDisplay}))");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"return (T)(object){mapping.ConvertToMethodName}(source);");
                    codeBuilder.EndSegment();
                }

                AppendNotSupportedExceptionAndEndSegment();
            }
            void AddToMethodForSingleWithPostAction()
            {
                codeBuilder.AppendCodeLines(
                   $"public static T {methodName}<T>(this {fromType.Display} source, Action<T> postHandler) where T: class, new()");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines($"var result = source.{methodName}<T>();");
                codeBuilder.AppendCodeLines("postHandler?.Invoke(result);");
                codeBuilder.AppendCodeLines("return result;");
                codeBuilder.EndSegment();
            }

            void AddCopyToMethodForSingle()
            {
                codeBuilder.AppendCodeLines(
                    $"public static void {methodName}<T>(this {fromType.Display} source, T target, Action<object> onRemoveItem = null, Action<object> onAddItem = null) where T:class");
                codeBuilder.BeginSegment();
                if (!fromType.Type.IsValueType)
                {
                    codeBuilder.AppendCodeLines("if (source == null) return;");
                }

                foreach (var mapping in mappingInfos)
                {
                    codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({mapping.TargetTypeFullDisplay}))");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"{mapping.ConvertToMethodName}(source, ({mapping.TargetTypeFullDisplay})(object)target, onRemoveItem, onAddItem);");
                    codeBuilder.AppendCodeLines($"return;");
                    codeBuilder.EndSegment();
                }

                AppendNotSupportedExceptionAndEndSegment();
            }
            void AddCopyToMethodForCollection()
            {
                codeBuilder.AppendCodeLines(
               $"public static void {methodName}<T>(this IEnumerable<{fromType.Display}> source, ICollection<T> target, Action<object> onRemoveItem = null, Action<object> onAddItem = null) where T: class, new()");
                codeBuilder.BeginSegment();
                foreach (var mapping in mappingInfos)
                {
                    codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({mapping.TargetTypeFullDisplay}))");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"{mapping.ConvertToMethodName}(source, (ICollection<{mapping.TargetTypeFullDisplay}>)target, onRemoveItem, onAddItem);");
                    codeBuilder.EndSegment();
                }

                AppendNotSupportedExceptionAndEndSegment();
            }

            void AddToMethodForEnumable()
            {
                codeBuilder.AppendCodeLines(
                    $"public static IEnumerable<T> {methodName}<T>(this IEnumerable<{fromType.Display}> source) where T:new()");
                codeBuilder.BeginSegment();
                foreach (var mapping in mappingInfos)
                {
                    codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({mapping.TargetTypeFullDisplay}))");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"return (IEnumerable<T>)source?.Select(p => p.{mapping.ConvertToMethodName}());");
                    codeBuilder.EndSegment();
                }

                AppendNotSupportedExceptionAndEndSegment();
            }
            void AddToMethodForEnumableWithPostAction()
            {

                codeBuilder.AppendCodeLines($"public static IEnumerable<T> {methodName}<T>(this IEnumerable<{fromType.Display}> source, Action<T> postHandler) where T : class, new()");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines("return source == null || postHandler == null ? source.To<T>() : source.To<T>().EachItem(postHandler);");
                codeBuilder.EndSegment();
            }

            void AddToMethodForQueryable()
            {
                codeBuilder.AppendCodeLines(
                    $"public static IQueryable<T> {methodName}<T>(this IQueryable<{fromType.Display}> source) where T:new()");
                codeBuilder.BeginSegment();
                foreach (var mapping in mappingInfos)
                {
                    codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({mapping.TargetTypeFullDisplay}))");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"return (IQueryable<T>){mapping.ConvertToMethodName}(source);");
                    codeBuilder.EndSegment();
                }

                AppendNotSupportedExceptionAndEndSegment();
            }
            void AppendNotSupportedExceptionAndEndSegment()
            {
                codeBuilder.AppendCodeLines($"throw new NotSupportedException($\"Can not convert '{{typeof({fromType.Display})}}' to '{{typeof(T)}}'.\");");
                codeBuilder.EndSegment();
            }
        }
    }
}
