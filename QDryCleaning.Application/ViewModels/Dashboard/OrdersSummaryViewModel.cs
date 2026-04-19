namespace QDryClean.Application.ViewModels.Dashboard
{
    public class OrdersSummaryViewModel
    {
        public PaymentRevenueViewModel Revenue { get; set; }
        public int TotalOrders { get; set; }
        public int ActiveOrders { get; set; } = 0;
        public int ReadyOrders { get; set; } = 0;
        public int CompletedOrders { get; set; } = 0;
    }
}
