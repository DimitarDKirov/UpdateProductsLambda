<h1>Project configuration steps</h1>
<h2>Setting up AWS access</h2>

1. Create an AWS root account.
2. Creat an IAM user with console and programmatic access via the AWS console with the root account. Make sure to **save** the access and secret keys for the created user.
3. Configure AWS credentials with the access and secret keys for the IAM user created above and a region set to **eu-central-1** using the command **aws configure --profile theProfileName**

<h2>AWS infrastructure and project configuration</h2>

1. Install Amazon.Lambda.Tools Global Tools if not already installed.

```
    dotnet tool install -g Amazon.Lambda.Tools
```
2. Open AWS Management console
3. Create an S3 bucket
4. Create a DynamoDB table called "dev-bg-products-table"
5. Create an IAM Role called **dev-bg-lambda-role** with the following permissions:
    - AmazonDynamoDBFullAccess
    - AmazonS3FullAccess
    - AmazonSESFullAccess
    - AWSStepFunctionsFullAccess
    - CloudWatchFullAccess
6. Open project root folder and set properties in **aws-lambda-tools-defaults.json**:
    - set **profile** to the profile name configured in **Setting up AWS access Step 3**
    - set **region** to **eu-central-1**
7. Build project
8. Publish project:
    - Open a terminal in the project root directory
    - Run **cd UpdateProductsLambda**
    - Build project
    - Run **dotnet lambda deploy-function theFunctionName --function-role dev-bg-lambda-role**
9. Open the published Lambda via the AWS Management console and add an **S3 trigger** in the **Configuration** tab. Trigger should specify S3 bucket created in **AWS infrastructure and project configuration Step 1**.

<h2>Test application</h2>

1. Open the S3 bucket created in **AWS infrastructure and project configuration Step 1**
2. Upload the **Static assets/Products.xlsx** file to S3 and confirm products are uploaded to the **dev-bg-products-table** database table
