namespace Erebor.Infrastructure.Data.BaseModels;

public class PagedList<T>
{
    public int TotalCount { get; set; }

    public int PageCount { get; set; }

    public int PageSize { get; set; }

    public int Page { get; set; }

    public List<T> Data { get; set; } = new List<T>();
}