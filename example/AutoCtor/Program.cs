using AutoCtor;

var c2 = new Service2(s => s.ToLower());

var c1 = new Service1(c2, "Hello");

c1.Run();

_ = new Demo1("val1",1);
_ = new Demo2("val2",2);
var demo = new Demo3(null,3);
