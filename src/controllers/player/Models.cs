public class ProductId
{
    public string sku { get; set; } = default!;
    public string name { get; set; } = default!;
}

public class Product
{
    public int amount { get; set; } = default!;
    public ProductId productId { get; set; } = default!;
    public string sku { get; set; } = default!;
    public string name { get; set; } = default!;
}

public class PublisherPayload
{
    public string appChargePaymentId { get; set; } = default!;
    public DateTime purchaseDateAndTimeUtc { get; set; } = default!;
    public string gameId { get; set; } = default!;
    public string playerId { get; set; } = default!;
    public string authType { get; set; } = default!;
    public string bundleName { get; set; } = default!;
    public string bundleId { get; set; } = default!;
    public string sku { get; set; } = default!;
    public int priceInCents { get; set; } = default!;
    public string currency { get; set; } = default!;
    public float priceInDollar { get; set; } = default!;
    public string action { get; set; } = default!;
    public string actionStatus { get; set; } = default!;
    public List<Product> products { get; set; } = default!;
    public string publisherToken { get; set; } = default!;
}