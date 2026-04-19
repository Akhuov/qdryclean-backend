namespace QDryClean.Application.ViewModels.Dashboard
{
    public class PaymentRevenueViewModel
    {
        public decimal Total { get; set; }
        public decimal Paid { get; set; }
        public decimal Unpaid { get; set; }
        public decimal PartiallyPaid { get; set; }
    }
}
