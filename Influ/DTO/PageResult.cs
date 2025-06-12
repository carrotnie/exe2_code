namespace Influencerhub.Commons.DTO;

public class PagedResult<T> where T : class
{
    public List<T>? Items { get; set; }
    public int TotalPages { get; set; }
}