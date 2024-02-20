using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlyTiger.Mapper.Analyzer;
using MakeConst.Test;
using Microsoft;
using VerifyCS = MakeConst.Test.CSharpCodeFixVerifier<FlyTiger.Mapper.Analyzer.MapperAnalyzer, FlyTiger.Mapper.CodeFix.MapperNotUsedCodeFix>;
using VerifyCS2 = MakeConst.Test.CSharpCodeFixVerifier<FlyTiger.Mapper.Analyzer.MapperAnalyzer, FlyTiger.Mapper.CodeFix.MissingMapperCodeFix>;

namespace Flytiger.UnitTest.MSTest
{
    [TestClass]
    public class MapperAnalyzerTest
    {
        [TestMethod]
        public async Task Test()
        {
            var code = @"
using System;
using System.Linq;
using System.Collections.Generic;
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

    public class UserInfo
    {
        public string Name { get; set; }
    }

    public partial class TUser
    {
        public string Name { get; set; }
    }
    [Mapper(typeof(TUser),typeof(UserInfo))]
[Mapper(typeof(TUser),typeof(TUser))]
    [Mapper(typeof(UserInfo),typeof(TUser))]
    [Mapper(typeof(TUser),typeof(UserInfo))]

    public class Test
    {
        public void Run()
        {
            var users = new TUser[]{new TUser()};
            var r = users.To<UserInfo>();
        }
    }

    static class MapperExtensions
    {
        private static IEnumerable<T> EachItem<T>(this IEnumerable<T> source, Action<T> handler)
        {
            foreach (var item in source)
            {
                handler(item);
                yield return item;
            }
        }
        private static global::FlyTiger.UserInfo ToFlyTigerUserInfo(this global::FlyTiger.TUser source)
        {
            if (source == null) return default;
            return new global::FlyTiger.UserInfo
            {
                Name = source.Name,
            };
        }
        private static void ToFlyTigerUserInfo(this global::FlyTiger.TUser source, global::FlyTiger.UserInfo target)
        {
            if (source == null) return;
            if (target == null) return;
            target.Name = source.Name;
        }
        private static IQueryable<global::FlyTiger.UserInfo> ToFlyTigerUserInfo(this IQueryable<global::FlyTiger.TUser> source)
        {
            return source?.Select(p => new global::FlyTiger.UserInfo
            {
                Name = p.Name,
            });
        }
        public static T To<T>(this global::FlyTiger.TUser source) where T:new()
        {
            if (source == null) return default;
            if (typeof(T) == typeof(global::FlyTiger.UserInfo))
            {
                return (T)(object)ToFlyTigerUserInfo(source);
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::FlyTiger.TUser)}' to '{typeof(T)}'."");
        }
        public static T To<T>(this global::FlyTiger.TUser source, Action<T> postHandler) where T:new()
        {
            var result = source.To<T>();
            postHandler?.Invoke(result);
            return result;
        }
        public static void To<T>(this global::FlyTiger.TUser source, T target) where T:class
        {
            if (source == null) return;
            if (typeof(T) == typeof(global::FlyTiger.UserInfo))
            {
                ToFlyTigerUserInfo(source, (global::FlyTiger.UserInfo)(object)target);
                return;
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::FlyTiger.TUser)}' to '{typeof(T)}'."");
        }
        public static IEnumerable<T> To<T>(this IEnumerable<global::FlyTiger.TUser> source) where T:new()
        {
            if (typeof(T) == typeof(global::FlyTiger.UserInfo))
            {
                return (IEnumerable<T>)source?.Select(p => p.ToFlyTigerUserInfo());
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::FlyTiger.TUser)}' to '{typeof(T)}'."");
        }
        public static IEnumerable<T> To<T>(this IEnumerable<global::FlyTiger.TUser> source, Action<T> postHandler) where T : new()
        {
            return source == null || postHandler == null ? source.To<T>() : source.To<T>().EachItem(postHandler);
        }
        public static IQueryable<T> To<T>(this IQueryable<global::FlyTiger.TUser> source) where T:new()
        {
            if (typeof(T) == typeof(global::FlyTiger.UserInfo))
            {
                return (IQueryable<T>)ToFlyTigerUserInfo(source);
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::FlyTiger.TUser)}' to '{typeof(T)}'."");
        }
    }
}
";
            await CSharpAnalyzerVerifier<MapperAnalyzer>.VerifyAnalyzerAsync(code);
        }

        [TestMethod]
        public async Task StringCouldBeConstant_Diagnostic()
        {
            var code = @"
using System;
using System.Linq;
using System.Collections.Generic;
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

    public class UserInfo
    {
        public string Name { get; set; }
    }

    public partial class TUser
    {
        public string Name { get; set; }
    }
    [Mapper(typeof(TUser),typeof(UserInfo))]
    [Mapper(typeof(TUser),typeof(UserInfo))]
[Mapper(typeof(TUser),typeof(TUser))]
    [Mapper(typeof(UserInfo),typeof(TUser))]
    public class Test
    {
        public void Run()
        {
            var users = new TUser[]{new TUser()};
            var r = users.To<UserInfo>();
        }
    }

    static class MapperExtensions
    {
        private static IEnumerable<T> EachItem<T>(this IEnumerable<T> source, Action<T> handler)
        {
            foreach (var item in source)
            {
                handler(item);
                yield return item;
            }
        }
        private static global::FlyTiger.UserInfo ToFlyTigerUserInfo(this global::FlyTiger.TUser source)
        {
            if (source == null) return default;
            return new global::FlyTiger.UserInfo
            {
                Name = source.Name,
            };
        }
        private static void ToFlyTigerUserInfo(this global::FlyTiger.TUser source, global::FlyTiger.UserInfo target)
        {
            if (source == null) return;
            if (target == null) return;
            target.Name = source.Name;
        }
        private static IQueryable<global::FlyTiger.UserInfo> ToFlyTigerUserInfo(this IQueryable<global::FlyTiger.TUser> source)
        {
            return source?.Select(p => new global::FlyTiger.UserInfo
            {
                Name = p.Name,
            });
        }
        public static T To<T>(this global::FlyTiger.TUser source) where T:new()
        {
            if (source == null) return default;
            if (typeof(T) == typeof(global::FlyTiger.UserInfo))
            {
                return (T)(object)ToFlyTigerUserInfo(source);
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::FlyTiger.TUser)}' to '{typeof(T)}'."");
        }
        public static T To<T>(this global::FlyTiger.TUser source, Action<T> postHandler) where T:new()
        {
            var result = source.To<T>();
            postHandler?.Invoke(result);
            return result;
        }
        public static void To<T>(this global::FlyTiger.TUser source, T target) where T:class
        {
            if (source == null) return;
            if (typeof(T) == typeof(global::FlyTiger.UserInfo))
            {
                ToFlyTigerUserInfo(source, (global::FlyTiger.UserInfo)(object)target);
                return;
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::FlyTiger.TUser)}' to '{typeof(T)}'."");
        }
        public static IEnumerable<T> To<T>(this IEnumerable<global::FlyTiger.TUser> source) where T:new()
        {
            if (typeof(T) == typeof(global::FlyTiger.UserInfo))
            {
                return (IEnumerable<T>)source?.Select(p => p.ToFlyTigerUserInfo());
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::FlyTiger.TUser)}' to '{typeof(T)}'."");
        }
        public static IEnumerable<T> To<T>(this IEnumerable<global::FlyTiger.TUser> source, Action<T> postHandler) where T : new()
        {
            return source == null || postHandler == null ? source.To<T>() : source.To<T>().EachItem(postHandler);
        }
        public static IQueryable<T> To<T>(this IQueryable<global::FlyTiger.TUser> source) where T:new()
        {
            if (typeof(T) == typeof(global::FlyTiger.UserInfo))
            {
                return (IQueryable<T>)ToFlyTigerUserInfo(source);
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::FlyTiger.TUser)}' to '{typeof(T)}'."");
        }
    }
}
";

            await VerifyCS.VerifyCodeFixAsync(code, @"
using System;

class Program
{
    static void Main()
    {
        const string s = ""abc"";
    }
}
");
        }

        [TestMethod]
        public async Task StringCouldBeConstant_Diagnostic2()
        {
            var code = @"
using System;
using System.Linq;
using System.Collections.Generic;
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

    public class UserInfo
    {
        public string Name { get; set; }
    }

    public partial class TUser
    {
        public string Name { get; set; }
    }
    public class Test
    {
        public void Run()
        {
            var users = new TUser[]{new TUser()};
            var r = users.To<UserInfo>();
        }
    }

    static class MapperExtensions
    {
        private static IEnumerable<T> EachItem<T>(this IEnumerable<T> source, Action<T> handler)
        {
            foreach (var item in source)
            {
                handler(item);
                yield return item;
            }
        }
        private static global::FlyTiger.UserInfo ToFlyTigerUserInfo(this global::FlyTiger.TUser source)
        {
            if (source == null) return default;
            return new global::FlyTiger.UserInfo
            {
                Name = source.Name,
            };
        }
        private static void ToFlyTigerUserInfo(this global::FlyTiger.TUser source, global::FlyTiger.UserInfo target)
        {
            if (source == null) return;
            if (target == null) return;
            target.Name = source.Name;
        }
        private static IQueryable<global::FlyTiger.UserInfo> ToFlyTigerUserInfo(this IQueryable<global::FlyTiger.TUser> source)
        {
            return source?.Select(p => new global::FlyTiger.UserInfo
            {
                Name = p.Name,
            });
        }
        public static T To<T>(this global::FlyTiger.TUser source) where T:new()
        {
            if (source == null) return default;
            if (typeof(T) == typeof(global::FlyTiger.UserInfo))
            {
                return (T)(object)ToFlyTigerUserInfo(source);
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::FlyTiger.TUser)}' to '{typeof(T)}'."");
        }
        public static T To<T>(this global::FlyTiger.TUser source, Action<T> postHandler) where T:new()
        {
            var result = source.To<T>();
            postHandler?.Invoke(result);
            return result;
        }
        public static void To<T>(this global::FlyTiger.TUser source, T target) where T:class
        {
            if (source == null) return;
            if (typeof(T) == typeof(global::FlyTiger.UserInfo))
            {
                ToFlyTigerUserInfo(source, (global::FlyTiger.UserInfo)(object)target);
                return;
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::FlyTiger.TUser)}' to '{typeof(T)}'."");
        }
        public static IEnumerable<T> To<T>(this IEnumerable<global::FlyTiger.TUser> source) where T:new()
        {
            if (typeof(T) == typeof(global::FlyTiger.UserInfo))
            {
                return (IEnumerable<T>)source?.Select(p => p.ToFlyTigerUserInfo());
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::FlyTiger.TUser)}' to '{typeof(T)}'."");
        }
        public static IEnumerable<T> To<T>(this IEnumerable<global::FlyTiger.TUser> source, Action<T> postHandler) where T : new()
        {
            return source == null || postHandler == null ? source.To<T>() : source.To<T>().EachItem(postHandler);
        }
        public static IQueryable<T> To<T>(this IQueryable<global::FlyTiger.TUser> source) where T:new()
        {
            if (typeof(T) == typeof(global::FlyTiger.UserInfo))
            {
                return (IQueryable<T>)ToFlyTigerUserInfo(source);
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::FlyTiger.TUser)}' to '{typeof(T)}'."");
        }
    }
}
";

            await VerifyCS2.VerifyCodeFixAsync(code, @"
using System;

class Program
{
    static void Main()
    {
        const string s = ""abc"";
    }
}
");
        }
    }
}
