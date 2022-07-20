namespace Domain.Entities;

/// <summary>
///     Order information
///     having at least the customer information is necessary
///     but to keep it simple and aligned with assessment project
///     I didn't add extra information
/// </summary>
public class Order
{
    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public double RequiredBinWidth { get; set; }
    public virtual IList<OrderedProduct> Products { get; set; } = new List<OrderedProduct>();
}