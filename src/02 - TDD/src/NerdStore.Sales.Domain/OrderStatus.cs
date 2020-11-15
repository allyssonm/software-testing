namespace NerdStore.Sales.Domain
{
    public enum OrderStatus
    {
        Draft = 0,
        Initialized = 1,
        Paid = 4,
        Delivered = 5,
        Cancelled = 6
    }
}
