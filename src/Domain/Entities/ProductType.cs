namespace Domain.Entities;

public class ProductType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double RequiredBinWidth { get; set; }
    public uint StackSize { get; set; } = 1;
    public virtual ICollection<OrderedProduct> OrderedProducts { get; set; } = new List<OrderedProduct>();
}