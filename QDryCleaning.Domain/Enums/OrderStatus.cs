namespace QDryClean.Domain.Enums
{
    public enum OrderStatus
    {
        Draft = 0,        // заказ создан, но еще не подтвержден (опционально)
        Created = 1,      // заказ оформлен и принят в работу
        InProgress = 2,   // заказ в процессе (стирка/чистка)
        Ready = 3,        // заказ готов, можно забирать
        Completed = 4,    // клиент забрал заказ (финал)
        Canceled = 5,      // заказ отменен
        Donated = 6
    }
}