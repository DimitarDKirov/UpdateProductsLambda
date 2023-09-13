using Amazon.DynamoDBv2.DataModel;

namespace UpdateProductsLambda.Models
{
    [DynamoDBTable("dev-bg-products-table")]
    internal class Product
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Type { get; set; }
        public int AvailableAfter { get; set; }
    }
}
