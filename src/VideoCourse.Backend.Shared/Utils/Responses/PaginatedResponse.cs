using VideoCourse.Backend.Shared.Utils.Pagination;

namespace VideoCourse.Backend.Shared.Utils.Responses;

public class PaginatedResponse<T> : BasePageableModel
{
    public IList<T> Items
    {
        get => _items ??= new List<T>();
        set => _items = value;
    }

    private IList<T>? _items;
}