namespace StrongMindExercise.Application.Errors;
public class CommonErrors
{
    public static readonly Error NameCannotBeNullOrBlank = new Error(ErrorCodes.NameCannotBeNullOrBlank, "The name cannot be null or blank");

    public static readonly Error NameCannotBeDuplicate = new Error(ErrorCodes.NameCannotBeDuplicate, "The name cannot be a duplicate");

    public static readonly Error CannotHaveDuplicateChildren = new Error(ErrorCodes.CannotHaveDuplicateChildren, "Cannot have duplicate children");

    public static readonly Error MustHaveAtLeastOneChild = new Error(ErrorCodes.MustHaveAtLeastOneChild, "Must have at least one child");

    public static Error ObjectCannotBeFound(string obj) => new Error(ErrorCodes.ObjectCannotBeFound, $"This {obj} cannot be found.");
}
