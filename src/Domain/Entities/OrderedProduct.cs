namespace Domain.Entities;

public class OrderedProduct
{
    public int Id { get; set; }
    public virtual Order? Order { get; set; }
    public int OrderId { get; set; }
    public virtual ProductType? ProductType { get; set; }
    public int ProductTypeId { get; set; }
    public int Quantity { get; set; }
}