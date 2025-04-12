using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.ProblemDetails.Models;
using Microsoft.AspNetCore.Http;

namespace VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.ProblemDetails;


/// <summary>
/// Represents problem details for internal server errors.
/// </summary>
/// <remarks>
/// This class provides detailed information about internal server errors that occur during API requests.
/// </remarks>
public class InternalServerErrorProblemDetails : ProblemDetailModel
{
    public InternalServerErrorProblemDetails(string detail)
    {
        Title = "Internal Server Error";
        Detail = "Internal Server Error";
        Status = StatusCodes.Status500InternalServerError;
    }
}