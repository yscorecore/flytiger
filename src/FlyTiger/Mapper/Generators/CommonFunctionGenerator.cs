﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FlyTiger.Mapper.Generators
{
    internal class CommonFunctionGenerator
    {
        public void AppendFunctions(CsharpCodeBuilder codeBuilder)
        {
            codeBuilder.AppendCodeLines(@"private static IEnumerable<T> EachItem<T>(this IEnumerable<T> source, Action<T> handler)
{
    foreach (var item in source)
    {
        handler?.Invoke(item);
        yield return item;
    }
}
private static void ForEach<T>(this IEnumerable<T> source, Action<T> handler)
{
    foreach (var item in source)
    {
        handler?.Invoke(item);
    }
}
");
        }
    }
}
