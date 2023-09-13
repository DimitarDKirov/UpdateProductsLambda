using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;

namespace UpdateProductsLambda.Helpers
{
    internal class S3Service
    {
        private readonly IAmazonS3 _amazonS3;

        public S3Service()
        {
            _amazonS3 = new AmazonS3Client();
        }
        public async Task<GetObjectResponse> GetS3Object(S3Event s3Event)
        {
            var eventData = s3Event.Records.First().S3;

            var request = new GetObjectRequest
            {
                BucketName = eventData.Bucket.Name,
                Key = eventData.Object.Key
            };

            return await _amazonS3.GetObjectAsync(request);
        }
    }
}
