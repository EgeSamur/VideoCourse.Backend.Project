﻿namespace VideoCourse.Backend.Shared.Utils.Results.Concrete;

public class ErrorResult : Result
{
    public ErrorResult(string message) : base(false, message)
    {
    }

    public ErrorResult() : base(false)
    {
    }
}