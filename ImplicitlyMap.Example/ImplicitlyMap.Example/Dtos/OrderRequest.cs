namespace ImplicitlyMap.Example.Dtos;
public record OrderRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
   
}