namespace ImplicitlyMap.Example.Dtos;
public record OrderResponse(int Id, string Name, DateTime OrderDate, decimal Quantity = 0);