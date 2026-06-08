namespace MunicipalityTaxService.Shared;

public class OperationResult
{
    protected OperationResult(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException("A successful result cannot contain an error.");
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("A failed result must contain an error.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static OperationResult Success() => new(true, Error.None);

    public static OperationResult Failure(Error error) => new(false, error);

    public static OperationResult<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static OperationResult<TValue> Failure<TValue>(Error error) => new(default, false, error);
}

public class OperationResult<TValue> : OperationResult
{
    private readonly TValue? _value;

    protected internal OperationResult(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failed result cannot be accessed.");

    public static implicit operator OperationResult<TValue>(TValue value) => Success(value);
}
