namespace MusicStore.Domain.Entities;

public class InventoryItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public int Year { get; set; }
    public string? Genre { get; set; }
    public decimal Price { get; set; }
}
