namespace carstore
{
    public class Car 
    {
        public int CarID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; } 
        public bool IsAvailable { get; set; }
    }
}