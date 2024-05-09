namespace Domain.Shared;

public abstract class BaseException : Exception
{
    public string Code { get; set; }
    public string Description { get; set; }
    public int? StatusCode { get; set; }

    public BaseException(string code, string description, int? statusCode)
    {
        Code = code;
        Description = description;
        StatusCode = statusCode;
    }
}
