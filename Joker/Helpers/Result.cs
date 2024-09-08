using System;

namespace Joker.Helpers;

public readonly struct Result<TSuccess, TFailure>
    where TSuccess : class
    where TFailure : class
{
    private readonly TSuccess? _success;
    private readonly TFailure? _failure;

    private Result(TSuccess? success, TFailure? failure)
    {
        _success = success;
        _failure = failure;
    }

    public static Result<TSuccess, TFailure> Success(TSuccess? success)
    {
        return new Result<TSuccess, TFailure>(success, null);
    }

    public static Result<TSuccess, TFailure> Failure(TFailure? failure)
    {
        return new Result<TSuccess, TFailure>(null, failure);
    }

    public TSuccess Unwrap()
    {
        if (_success == null) throw new InvalidOperationException("Can not unrwap failed result");

        return _success;
    }

    public void Handle(Action<TSuccess> success, Action<TFailure> failure)
    {
        if (_success != null) success(_success);

        if (_failure != null) failure(_failure);
    }
}