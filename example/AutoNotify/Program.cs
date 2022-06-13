// See https://aka.ms/new-console-template for more information
using System;
using AutoNotify;

var s1 = new Service1();
s1.PropertyChanged += (s, e) => { Console.WriteLine(e.PropertyName); };
s1.Value = 100;
