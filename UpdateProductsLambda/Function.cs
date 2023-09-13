using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.Lambda.Serialization.SystemTextJson;
using UpdateProductsLambda.Helpers;
using UpdateProductsLambda.Services;

namespace UpdateProductsLambda;

public class Function
{
    [LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
    public async Task<string> FunctionHandler(S3Event s3Event)
    {
        var s3Service = new S3Service();
        var productService = new ProductService();

        var s3Object = await s3Service.GetS3Object(s3Event);

        var products = productService.ExtractProductsFromExcelSheet(s3Object.ResponseStream);

        await productService.ClearDynamo();

        await productService.UploadProductsToDynamo(products.ToList());

        return "Success";
    }
}
