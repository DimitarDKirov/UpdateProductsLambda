using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using OfficeOpenXml;
using UpdateProductsLambda.Models;

namespace UpdateProductsLambda.Services
{
    internal class ProductService
    {
        private readonly IDynamoDBContext _dbContext;

        public ProductService()
        {
            var client = new AmazonDynamoDBClient();
            _dbContext = new DynamoDBContext(client);
        }

        public async Task ClearDynamo()
        {
            var scanConditions = new List<ScanCondition>();
            var search = _dbContext.ScanAsync<Product>(scanConditions);
            var searchCount = 0;

            var batchDelete = _dbContext.CreateBatchWrite<Product>();

            while (!search.IsDone)
            {
                var batch = await search.GetNextSetAsync();
                searchCount += batch.Count;
                batchDelete.AddDeleteItems(batch);
            }

            await batchDelete.ExecuteAsync();
        }

        public async Task UploadProductsToDynamo(List<Product> products)
        {
            var batchWrite = _dbContext.CreateBatchWrite<Product>();
            batchWrite.AddPutItems(products);

            await batchWrite.ExecuteAsync();
        }

        public IEnumerable<Product> ExtractProductsFromExcelSheet(Stream responseStream)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var excelPackage = new ExcelPackage(responseStream))
            {
                var worksheet = excelPackage.Workbook.Worksheets[0];
                var row = 2;
                var column = 1;

                var productsList = new List<Product>();

                while (worksheet.Cells[row, column].Value != null)
                {
                    var product = new Product();
                    while (worksheet.Cells[row, column].Value != null)
                    {
                        var cellValue = worksheet.Cells[row, column].Value;
                        var cellHeader = worksheet.Cells[1, column].Value.ToString();
                        var properties = typeof(Product).GetProperties();

                        var value = cellValue.GetType().Name.ToLower() == "double" ?
                            Convert.ToInt32(cellValue) : cellValue;

                        properties.FirstOrDefault(p => p.Name.ToLower() == cellHeader.ToLower())?.SetValue(product, value);

                        column++;
                    }

                    productsList.Add(product);
                    column = 1;
                    row++;
                }

                return productsList;
            }
        }
    }
}
