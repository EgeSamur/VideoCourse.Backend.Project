using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.ProblemDetails.Models;
using Microsoft.AspNetCore.Http;

namespace VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.ProblemDetails;

/// <summary>
/// Represents problem details for business logic errors.
/// </summary>
/// <remarks>
/// This class provides detailed information about business logic errors that occur during API requests.
/// </remarks>
public class BusinessProblemDetails : ProblemDetailModel
{
    public BusinessProblemDetails(string detail)
    {
        Title = "Business Error";
        Detail = detail;
        Status = StatusCodes.Status400BadRequest;
    }
}