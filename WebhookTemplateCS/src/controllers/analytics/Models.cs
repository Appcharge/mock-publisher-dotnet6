namespace WebhookTemplateCS.controllers.analytics;

public class GetAnalyticsRequest
{
    public string startDate { get; set; } = default!;
    public string endDate { get; set; } = default!;
    public List<string> metrics { get; set; } = default!;
    public string incomeType { get; set; } = default!;
}
