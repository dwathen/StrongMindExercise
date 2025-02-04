namespace StrongMindExercise.Application.Errors;
public static class ErrorCodes
{
    public const string NameCannotBeNullOrBlank = "Common.NameCannotBeNullOrBlank";
    public const string NameCannotBeDuplicate = "Common.NameCannotBeDuplicate";
    public const string ObjectAlreadyExists = "Common.ObjectAlreadyExists";
    public const string ObjectCannotBeFound = "Common.ObjectCannotBeFound";
    public const string CannotHaveDuplicateChildren = "Common.CannotHaveDuplicateChildren";
    public const string MustHaveAtLeastOneChild = "Common.MustHaveAtLeastOneChild";
}
