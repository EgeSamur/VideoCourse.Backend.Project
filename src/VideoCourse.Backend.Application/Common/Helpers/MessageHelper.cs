namespace VideoCourse.Backend.Application.Common.Helpers;

public static class MessageHelper
{
    public static string NotFound(string entityName)
    {
        return $"{entityName} not found.";
    }
    
    public static string AlreadyExists(string entityName)
    {
        return $"{entityName} already exists.";
    }
    
    public static string Listed(string entityName)
    {
        return $"{entityName} listed successfully.";
    }
    
    public static string FetchedById(string entityName)
    {
        return $"{entityName} fetched successfully.";
    }
    
    public static string Created(string entityName)
    {
        return $"{entityName} created successfully.";
    }
    
    public static string Updated(string entityName)
    {
        return $"{entityName} updated successfully.";
    }
    
    public static string Deleted(string entityName)
    {
        return $"{entityName} deleted successfully.";
    }
    public static string Setted(string entityName)
    {
        return $"{entityName} setted successfully.";
    }
    public static string Buyyed(string entityName)
    {
        return $"{entityName} buyyed successfully.";
    }
    public static string DoesNotHaveAUserRight(string entityName)
    {
        return $"{entityName} does not have a right.";
    }
    public static string Boosted(string entityName, int boostTime)
    {
        return $"{entityName} boosted for {boostTime} minutes.";
    }

    public static string Sent(string entityName)
    {
        return $"{entityName} sent.";
    }
    public static string Invalid(string entityName)
    {
        return $"Invalid {entityName} ";
    }

}