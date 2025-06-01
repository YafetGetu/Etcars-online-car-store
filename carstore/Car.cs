namespace carstore
{
    public class Car // Ensure the class is public
    {
        public int CarID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; } // To store the Base64 string
        public bool IsAvailable { get; set; }
    }
}