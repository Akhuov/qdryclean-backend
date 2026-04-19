namespace QDryClean.Domain.Enums
{
    public enum PaymentMethod
    {
        Cash = 1,        // Наличные
        Card = 2,        // Банковская карта (POS)
        Click = 3,       // Click
        Payme = 4,       // Payme
        Uzum = 5,        // Uzum Bank / Uzum Pay
        Other = 99
    }
}
