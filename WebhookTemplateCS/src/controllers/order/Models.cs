namespace WebhookTemplateCS.controllers.order;

public class OfferSnapshotProduct
{
    public string name { get; set; } = default!;
    public string sku { get; set; } = default!;
    public int amount { get; set; } = default!;
}

public class OfferSnapshot
{
    public string publisherId { get; set; } = default!;
    public string offerId { get; set; } = default!;
    public string name { get; set; } = default!;
    public string sku { get; set; } = default!;
    public List<OfferSnapshotProduct> products { get; set; } = new List<OfferSnapshotProduct>();
    public float price { get; set; } = default!;
    public string currencyCode { get; set; } = default!;
    public string description { get; set; } = default!;
}

public class GetOrdersResponse
{
    public int totalCount { get; set; } = default!;
    public List<OrderItemResponse> results { get; set; } = new List<OrderItemResponse>();
}
public class GetOrdersRequest
{
    public string startDate { get; set; } = default!;
    public string endDate { get; set; } = default!;
    public int recordLimit { get; set; } = default!;
    public int offset { get; set; } = default!;
    public List<string> statuses { get; set; } = default!;
}
public class OrderItemResponse
{
    public string id { get; set; } = default!;
    public string publisherId { get; set; } = default!;
    public string paymentId { get; set; } = default!;
    public string paymentUrl { get; set; } = default!;
    public string publisherPurchaseId { get; set; } = default!;
    public string currency { get; set; } = default!;
    public int amountTotal { get; set; } = default!;
    public string currencySymbol { get; set; } = default!;
    public string offersetId { get; set; } = default!;
    public string bundleName { get; set; } = default!;
    public string provider { get; set; } = default!;
    public string utmSource { get; set; } = default!;
    public string utmMedium { get; set; } = default!;
    public string utmCampaign { get; set; } = default!;
    public string modifiedAt { get; set; } = default!;
    public string createdAt { get; set; } = default!;
    public string playerId { get; set; } = default!;
    public string clientGaId { get; set; } = default!;
    public string state { get; set; } = default!;
    public string reason { get; set; } = default!;
    public string publisherErrorMessage { get; set; } = default!;
    public int retry { get; set; } = default!;
    public OfferSnapshot? offerSnapshot { get; set; }
}