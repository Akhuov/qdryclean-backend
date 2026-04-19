using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.ViewModels.Dashboard;

namespace QDryClean.Application.UseCases.Dashboard.Queries
{
    public class OrdersSummaryQuery : IRequest<ApiResponse<OrdersSummaryViewModel>> 
    {
        public string From { get; set; }
        public string To { get; set; }
    }
}
