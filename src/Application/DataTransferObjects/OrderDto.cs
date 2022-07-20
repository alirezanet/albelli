namespace Application.DataTransferObjects;

public record OrderDto(
    int Id,
    double RequiredBinWidth,
    IList<OrderedProductDto> Products);