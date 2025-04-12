using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.ProblemDetails.Models;
using Microsoft.AspNetCore.Http;

namespace VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.ProblemDetails;

/// <summary>
/// Represents problem details for authorization errors.
/// </summary>
/// <remarks>
/// This class provides detailed information about authorization errors that occur during API requests.
/// </remarks>
public class AuthorizationProblemDetails : ProblemDetailModel
{
    public AuthorizationProblemDetails(string detail)
    {
        Title = "Authorization Error";
        Detail = detail;
        Status = StatusCodes.Status401Unauthorized;
    }
}