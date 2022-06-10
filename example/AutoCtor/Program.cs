using AutoCtor;

var c2 = new Service2(s => s.ToLower());

var c1 = new Service1(c2, "Hello");

c1.Run();


