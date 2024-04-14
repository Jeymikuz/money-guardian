namespace money.guardian.core.common.errors;

public class BaseError(string message)
{
    public string Message { get; } = message;
}