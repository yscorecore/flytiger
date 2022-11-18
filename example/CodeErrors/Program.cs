// See https://aka.ms/new-console-template for more information
using CodeErrors;

var exception = AllErrors.UserNotFound("张三");
Console.WriteLine(exception.Message);
Console.WriteLine(exception.Code);
