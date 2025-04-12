namespace VideoCourse.Backend.Shared.Utils.Results.Abstract;

public interface IResult
{
    bool Success { get; }
    string Message { get; }
}