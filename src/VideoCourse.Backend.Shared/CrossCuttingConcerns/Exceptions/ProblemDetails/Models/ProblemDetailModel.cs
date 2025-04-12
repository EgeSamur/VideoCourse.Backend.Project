namespace VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.ProblemDetails.Models;


/// <summary>
/// Represents a model for problem details in the API.
/// </summary>
/// <remarks>
/// This model is used to provide detailed information about errors that occur during API requests.
/// </remarks>
public class ProblemDetailModel
{
    public string Title { get; set; }
    public string Detail { get; set; }
    public int Status { get; set; }
}