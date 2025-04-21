namespace EventSourcingSample.WebAPI.Features.Orders.Application.Shared
{
    public record ProductQuantity(int ProductId, int Quantity);

    public record DeliveryDetails(string Street, string City, string Country);

    public record PaymentInformation(string CardNumber, string ExpireDate, string Security);

    public record ProductQuantityName(int ProductId, int Quantity, string Name);
}
