using System;
using System.Collections.Generic;
using System.Text;

namespace FlyTiger
{
    partial class MapperGenerator
    {
        const string EFCoreQueryableExtensionsName = "EFCoreQueryableExtensions";

        internal static string EFCoreQueryableExtensionsFullName = $"{NameSpaceName}.{EFCoreQueryableExtensionsName}";

        const string EFCoreExtensionsCode = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FlyTiger
{
    internal static class EFCoreQueryableExtensions
    {
        public static IQueryable<TResult> RebuildWithIncludeForEfCore<TResult>(this IQueryable<TResult> toQuery)
        {
            if (toQuery?.Expression is MethodCallExpression callExpression
                && callExpression.Method.Name == nameof(Queryable.Select) && callExpression.Method.DeclaringType == typeof(Queryable)
                && callExpression.Arguments.Count == 2)
            {
                var includePaths = FindIncludeCollectionLambda(callExpression.Arguments[0]);
                if (includePaths.Any())
                {
                    return toQuery.Provider.CreateQuery<TResult>(Expression.Call(
                         null,
                         callExpression.Method,
                         callExpression.Arguments[0], ReplaceSelectCollectionExpression(callExpression.Arguments[1], includePaths)
                     ));
                }
            }
            return toQuery;
        }
        static IList<MemberIncludePath> FindIncludeCollectionLambda(Expression expression)
        {
            var includeFinder = new IncludePathFinder();
            includeFinder.Visit(expression);
            return includeFinder.MemberIncludePaths;
        }
        static Expression ReplaceSelectCollectionExpression(Expression expression, IList<MemberIncludePath> includePaths)
        {
            var visitCollectionSelect = new CollectionSelectReplacer(includePaths);
            return visitCollectionSelect.Visit(expression);
        }
#pragma warning disable CS8603,CS8604,CS8618
        class IncludePathFinder : ExpressionVisitor
        {
            public List<MemberIncludePath> MemberIncludePaths { get; private set; } = new List<MemberIncludePath>();
            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if ((node.Method.Name == ""Include"" || node.Method.Name == ""ThenInclude"") && node.Type.IsGenericType &&
                    node.Type.GetGenericTypeDefinition().FullName == ""Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2"")
                {
                    var quoteExpression = node.Arguments[1] as UnaryExpression;
                    var lambda = quoteExpression?.Operand as LambdaExpression;
                    var memberIncludePath = FindMemberExpression(lambda);
                    if (memberIncludePath != null)
                    {
                        MemberIncludePaths.Add(memberIncludePath);
                    }
                }
                return base.VisitMethodCall(node);
            }
            private MemberIncludePath FindMemberExpression(LambdaExpression lambdaExpression)
            {
                if (lambdaExpression == null) return null;
                if (lambdaExpression.ReturnType.IsGenericType == false)
                {

                    return null;
                }

                var itemType = GetItemType(lambdaExpression.ReturnType);
                if (itemType == null)
                {
                    return null;
                }
                MemberIncludePath result = new MemberIncludePath();
                var expression = lambdaExpression.Body;
                while (true)
                {
                    if (expression is MemberExpression member)
                    {
                        var memberItemType = GetItemType(member.Type);
                        if (memberItemType == itemType)
                        {
                            result.ItemType = memberItemType;
                            result.Member = member;
                            return result.HasCallPath ? result : null;
                        }
                        else
                        {
                            return null;
                        }

                    }
                    else if (expression is ParameterExpression)
                    {
                        return null;
                    }
                    else if (expression is MethodCallExpression method)
                    {
                        result.CallPaths.Insert(0, method);
                        expression = method.Arguments[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                Type GetItemType(Type sourceType)
                {
                    if (sourceType.IsArray)
                    {
                        return sourceType.GetElementType();
                    }
                    if (sourceType.IsGenericType && sourceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        return sourceType.GetGenericArguments().First();
                    }
                    return sourceType.GetInterfaces()
                          .Where(p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                          .Select(p => p.GetGenericArguments().First()).FirstOrDefault();
                }
            }
        }

        class CollectionSelectReplacer : ExpressionVisitor
        {
            private readonly IDictionary<MemberInfo, MemberIncludePath> memberPathsMap = new Dictionary<MemberInfo, MemberIncludePath>();

            public CollectionSelectReplacer(IEnumerable<MemberIncludePath> memberIncludePaths)
            {
                foreach (var memberIncludePath in memberIncludePaths)
                {
                    memberPathsMap[memberIncludePath.Member.Member] = memberIncludePath;
                }
            }
            protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
            {
                if (node.Expression is MemberExpression member && memberPathsMap.TryGetValue(member.Member, out var paths))
                {
                    return node.Update(ReplaceAndConvertExpression(member, paths));
                }

                return base.VisitMemberAssignment(node);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                var supportNames = new string[] {
                        nameof(Enumerable.ToList),
                        nameof(Enumerable.ToArray),
                        nameof(Enumerable.AsEnumerable),
                        nameof(Enumerable.Select),
                    };
                if (node.Method.DeclaringType == typeof(Enumerable) && supportNames.Contains(node.Method.Name) && node.Arguments[0] is MemberExpression member && memberPathsMap.TryGetValue(member.Member, out var paths))
                {
                    var arguments = node.Arguments.ToArray();
                    arguments[0] = ReplaceExpression(member, paths);
                    return Expression.Call(null, node.Method, arguments);
                }

                return base.VisitMethodCall(node);
            }
            private Expression ReplaceExpression(MemberExpression expression, MemberIncludePath paths)
            {
                Expression exp = expression;
                foreach (var callPath in paths.CallPaths)
                {
                    var arguments = callPath.Arguments.ToArray();
                    arguments[0] = exp;
                    exp = Expression.Call(null, callPath.Method, arguments);
                }
                return exp;
            }
            static MethodInfo ToArrayMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToArray));
            static MethodInfo ToListMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToList));
            private Expression ReplaceAndConvertExpression(MemberExpression expression, MemberIncludePath paths)
            {
                var returnType = expression.Type;
                Expression exp = expression;
                Type expressionReturnType = expression.Type;
                foreach (var callPath in paths.CallPaths)
                {
                    var arguments = callPath.Arguments.ToArray();
                    arguments[0] = exp;
                    exp = Expression.Call(null, callPath.Method, arguments);
                    expressionReturnType = callPath.Method.ReturnType;
                }
                if (expressionReturnType == returnType)
                {
                    return exp;
                }
                else if (returnType.IsArray)
                {
                    return Expression.Call(null, ToArrayMethod.MakeGenericMethod(paths.ItemType), exp);
                }
                else if (returnType.IsAssignableFrom(typeof(List<>).MakeGenericType(paths.ItemType)))
                {
                    return Expression.Call(null, ToListMethod.MakeGenericMethod(paths.ItemType), exp);
                }
                else
                {
                    return expression;
                }
            }
        }
        class MemberIncludePath
        {
            public Type ItemType { get; set; }
            public MemberExpression Member { get; set; }
            public List<MethodCallExpression> CallPaths { get; } = new List<MethodCallExpression>();
            public bool HasCallPath { get => CallPaths.Any(); }
        }
    }
#pragma warning restore CS8603,CS8604,CS8618 
}
";
    }
}
