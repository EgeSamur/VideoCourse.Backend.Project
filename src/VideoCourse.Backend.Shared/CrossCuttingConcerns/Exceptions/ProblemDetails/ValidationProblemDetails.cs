using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.ProblemDetails.Models;
using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Types;
using Microsoft.AspNetCore.Http;

namespace VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.ProblemDetails;

/// <summary>
/// Represents problem details for validation errors.
/// </summary>
/// <remarks>
/// This class provides detailed information about validation errors that occur during API requests.
/// </remarks>
public class ValidationProblemDetails : ProblemDetailModel
{
    public IEnumerable<ValidationExceptionModel> Errors { get; init; }

    public ValidationProblemDetails(IEnumerable<ValidationExceptionModel> errors)
    {
        Title = "Validation Error(s)";
        Detail = "One or more validation errors occurred.";
        Errors = errors;
        Status = StatusCodes.Status400BadRequest;
    }
}