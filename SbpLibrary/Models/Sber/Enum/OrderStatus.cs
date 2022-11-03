namespace SBPLibrary.Models.Sber
{
    public enum OrderStatus
    {
        Paid, 
        Created,
        Reversed,
        Refunded,
        Revoked, 
        Declined,
        Expired, 
        Authorized,
        Confirmed,
        OnPayment
    }
}