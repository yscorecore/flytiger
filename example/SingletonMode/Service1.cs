﻿using System;
using FlyTiger;

namespace SingletonMode
{
    [SingletonPattern]
    public  partial class Service1
    {
        public void SayHello(string name)
        {
            Console.WriteLine($"Hello,{name}");
        }
    }

}
