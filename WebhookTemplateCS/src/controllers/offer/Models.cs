namespace WebhookTemplateCS.controllers.offer;

public class CreateOfferRequest
{
    public string createdBy { get; set; } = default!;
    public string publisherOfferId { get; set; } = default!;
    public string name { get; set; } = default!;
    public string description { get; set; } = default!;
    public string type { get; set; } = default!;
    public List<Intervals> intervals { get; set; } = default!;
    public string offerUiId { get; set; } = default!;
    public DynamicOfferUi dynamicOfferUi { get; set; } = default!;
    public bool active { get; set; } = default!;
    public int? coolDownInHours { get; set; } = default!;
    public List<string> segments { get; set; } = default!;
    public List<ProductSequence> productsSequence { get; set; } = default!;
}

public class DynamicOfferUi
{
    public List<Badges> badges { get; set; } = default!;
    public int? salePercentage { get; set; } = default!;
}

public class Intervals
{
    public string startDate { get; set; } = default!;
    public string endDate { get; set; } = default!;
}

public class Badges
{
    public string publisherBadgeId { get; set; } = default!;
    public string position { get; set; } = default!;
}

public class ProductSequence
{
    public int index { get; set; } = default!;
    public List<Product> products { get; set; } = default!;
}

public class Product
{
    public string publisherProductId { get; set; } = default!;
    public int quantity { get; set; } = default!;
}
