namespace StrongMindExercise.Application.Errors;
public sealed record class Error
{
    public string Code { get; set; }
    public string Description { get; set; }

    public Error(string code, string description)
    {
        this.Code = code;
        this.Description = description;
    }

    public static readonly Error None = new(string.Empty, string.Empty);
}
