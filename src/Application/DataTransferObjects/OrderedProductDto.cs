namespace Application.DataTransferObjects;

public record OrderedProductDto
{
    public string ProductType { get; set; } = string.Empty;
    public int Quantity { get; set; }
}