﻿namespace VideoCourse.Backend.Shared.Utils.Results.Concrete;

public class SuccessResult : Result
{
    public SuccessResult(string message) : base(true, message)
    {
    }

    public SuccessResult() : base(true)
    {
    }
}