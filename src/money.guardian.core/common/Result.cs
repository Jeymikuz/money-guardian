using money.guardian.core.common.errors;

namespace money.guardian.core.common;

public class Result<T>
{
    public Result(T value)
    {
        Value = value;
        Error = default;
    }

    public Result(BaseError error)
    {
        Value = default;
        Error = error;
    }

    public T Value { get; }
    public BaseError Error { get; }
    public bool IsError => Error is not null;

    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(BaseError baseError) => new(baseError);
}