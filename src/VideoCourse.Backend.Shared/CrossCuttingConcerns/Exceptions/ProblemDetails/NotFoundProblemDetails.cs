using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.ProblemDetails.Models;
using Microsoft.AspNetCore.Http;

namespace VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.ProblemDetails;


/// <summary>
/// Represents problem details for not found errors.
/// </summary>
/// <remarks>
/// This class provides detailed information about not found errors that occur during API requests.
/// </remarks>
public class NotFoundProblemDetails : ProblemDetailModel
{
    public NotFoundProblemDetails(string detail)
    {
        Title = "Not Found";
        Detail = detail;
        Status = StatusCodes.Status404NotFound;
    }
}