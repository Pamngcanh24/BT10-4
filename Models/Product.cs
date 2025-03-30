namespace ProductApi.Models
{
   public class Product
{
    public int ProductId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int Stock { get; set; }
    public required string Category { get; set; }
    public required string Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}

}
