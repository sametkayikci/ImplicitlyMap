namespace ImplicitlyMap.Example.Entity;

public record Order(int Id, string Name, DateTime OrderDate, int Quantity = 0);