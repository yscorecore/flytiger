using FlyTiger;
using FlyTiger.CodeException;
namespace CodeErrors
{
    [CodeExceptions(CodePrefix = "SCH_")]
    internal static partial class AllErrors
    {
        [CodeException("10001", "user '{name}' not found.")]
        public static partial CodeException UserNotFound(string name, int age = 1);
    }
}
