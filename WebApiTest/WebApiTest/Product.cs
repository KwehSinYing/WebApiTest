namespace WebApiTest
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }

        public string ProductDesc { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}