namespace QDryClean.Application.Common.Interfaces
{
    public interface IPagedQuery
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }
}
