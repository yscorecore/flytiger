﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlyTiger.Mapper.Analyzer;
using FlyTiger.UnitTest.Verifiers;
using Xunit;

namespace FlyTiger.UnitTest
{
    public class MapperAnalyzerTest
    {
        [Fact]
        public async Task LocalIntCouldBeConstant_Diagnostic()
        {
            var code = @"
namespace ConsoleApp2
{
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
            var r =new TUser().To<UserInfo>();
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
        private static global::ConsoleApp2.UserInfo ToConsoleApp2UserInfo(this global::ConsoleApp2.TUser source)
        {
            if (source == null) return default;
            return new global::ConsoleApp2.UserInfo
            {
                Name = source.Name,
            };
        }
        private static void ToConsoleApp2UserInfo(this global::ConsoleApp2.TUser source, global::ConsoleApp2.UserInfo target)
        {
            if (source == null) return;
            if (target == null) return;
            target.Name = source.Name;
        }
        private static IQueryable<global::ConsoleApp2.UserInfo> ToConsoleApp2UserInfo(this IQueryable<global::ConsoleApp2.TUser> source)
        {
            return source?.Select(p => new global::ConsoleApp2.UserInfo
            {
                Name = p.Name,
            });
        }
        public static T To<T>(this global::ConsoleApp2.TUser source) where T:new()
        {
            if (source == null) return default;
            if (typeof(T) == typeof(global::ConsoleApp2.UserInfo))
            {
                return (T)(object)ToConsoleApp2UserInfo(source);
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::ConsoleApp2.TUser)}' to '{typeof(T)}'."");
        }
        public static T To<T>(this global::ConsoleApp2.TUser source, Action<T> postHandler) where T:new()
        {
            var result = source.To<T>();
            postHandler?.Invoke(result);
            return result;
        }
        public static void To<T>(this global::ConsoleApp2.TUser source, T target) where T:class
        {
            if (source == null) return;
            if (typeof(T) == typeof(global::ConsoleApp2.UserInfo))
            {
                ToConsoleApp2UserInfo(source, (global::ConsoleApp2.UserInfo)(object)target);
                return;
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::ConsoleApp2.TUser)}' to '{typeof(T)}'."");
        }
        public static IEnumerable<T> To<T>(this IEnumerable<global::ConsoleApp2.TUser> source) where T:new()
        {
            if (typeof(T) == typeof(global::ConsoleApp2.UserInfo))
            {
                return (IEnumerable<T>)source?.Select(p => p.ToConsoleApp2UserInfo());
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::ConsoleApp2.TUser)}' to '{typeof(T)}'."");
        }
        public static IEnumerable<T> To<T>(this IEnumerable<global::ConsoleApp2.TUser> source, Action<T> postHandler) where T : new()
        {
            return source == null || postHandler == null ? source.To<T>() : source.To<T>().EachItem(postHandler);
        }
        public static IQueryable<T> To<T>(this IQueryable<global::ConsoleApp2.TUser> source) where T:new()
        {
            if (typeof(T) == typeof(global::ConsoleApp2.UserInfo))
            {
                return (IQueryable<T>)ToConsoleApp2UserInfo(source);
            }
            throw new NotSupportedException($""Can not convert '{typeof(global::ConsoleApp2.TUser)}' to '{typeof(T)}'."");
        }
    }
}


";
            await CSharpAnalyzerVerifier<MapperAnalyzer>.VerifyAnalyzerAsync(code);
        }
    }
}
