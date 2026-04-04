namespace sprintFlow.Application.Common;

public class PagedUsers<U>
{
    public PagedUsers(IEnumerable<U> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items;
        TotalItemsCount = totalCount;
        TotalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);
        ItemsFrom = totalCount == 0 ? 0 : ((pageNumber - 1) * pageSize) + 1;
        ItemsTo = totalCount == 0 ? 0 : Math.Min(pageNumber * pageSize, TotalItemsCount);
    }
    public IEnumerable<U> Items { get; set; }
    public int TotalItemsCount { get; set; }
    public int TotalPages { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
}
