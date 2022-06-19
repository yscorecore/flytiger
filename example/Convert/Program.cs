// See https://aka.ms/new-console-template for more information

using System;
using System.Text.Json;
using System.Threading.Channels;
using Convert;

var from = new From { StrProp = "hello", IntProp = 2 };
var to = from.To<To>();
Console.WriteLine(JsonSerializer.Serialize(to));
