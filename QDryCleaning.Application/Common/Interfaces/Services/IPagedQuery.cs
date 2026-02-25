namespace QDryClean.Application.Common.Interfaces.Services
{
    public interface IPagedQuery
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }
}
