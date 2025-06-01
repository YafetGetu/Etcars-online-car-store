// In a new file named OrderDisplayItem.cs or within DatabaseConnection.cs namespace
public class OrderDisplayItem
{
    public int OrderID { get; set; }
    public string Car { get; set; } // Combination of Brand, Model, Year
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
}