using QDryClean.Domain.Enums;

namespace QDryClean.Api.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int ReceiptNumber { get; set; }
        public ProcessStatus ProcessStatus { get; set; }
        public DateOnly ExpectedCompletionDate { get; set; }
        public DateOnly CreatedAt { get; set; }
        public int ItemsCount { get; set; }
        public IList<string> Notes { get; set; } = new List<string>();

    }
}
