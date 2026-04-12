namespace QDryClean.Domain.Enums
{
    public enum ItemStatus
    {
        Accepted = 0,        // приняли вещь (создание)
        Packed = 1,          // упаковано (готово к выдаче)
        Issued = 2,          // выдано клиенту

        Reprocessing = 3,    // повторная обработка

        Damaged = 4,         // повреждено
        Lost = 5             // потеряно
    }
}
