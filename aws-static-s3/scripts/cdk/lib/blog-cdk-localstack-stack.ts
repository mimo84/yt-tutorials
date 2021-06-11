import { AttributeType, Table, BillingMode } from '@aws-cdk/aws-dynamodb';
import { Construct, RemovalPolicy, Stack, StackProps } from '@aws-cdk/core';
import { NodejsFunction } from '@aws-cdk/aws-lambda-nodejs';
import { LambdaRestApi, LambdaIntegration } from '@aws-cdk/aws-apigateway';
import { Runtime } from '@aws-cdk/aws-lambda';

const lambdaPath = `${__dirname}/lambda`;

export class BlogCdkLocalstackStack extends Stack {
  constructor(scope: Construct, id: string, props?: StackProps) {
    super(scope, id, props);

    const modelName = 'Beer';

    const dynamoTable = new Table(this, modelName, {
      billingMode: BillingMode.PAY_PER_REQUEST,
      partitionKey: {
        name: `UserId`,
        type: AttributeType.STRING,
      },
      sortKey: {
        name: `${modelName}Id`,
        type: AttributeType.STRING,
      },
      removalPolicy: RemovalPolicy.DESTROY,
      tableName: modelName,
    });

    const updateReview = new NodejsFunction(this, 'updateReviewFunction', {
      entry: `${lambdaPath}/update-review.ts`,
      handler: 'handler',
      runtime: Runtime.NODEJS_14_X,
    });

    const getReviews = new NodejsFunction(this, 'readReviewFunction', {
      entry: `${lambdaPath}/get-reviews.ts`,
      handler: 'handler',
      runtime: Runtime.NODEJS_14_X,
    });

    dynamoTable.grantReadWriteData(updateReview);
    dynamoTable.grantReadData(getReviews);

    const api = new LambdaRestApi(this, 'beer-api', {
      handler: getReviews,
      proxy: false,
    });

    const users = api.root.addResource('users');
    users.addMethod('GET', new LambdaIntegration(getReviews));
    const user = users.addResource('{user}');
    const userReview = user.addResource('{beer}');
    userReview.addMethod('POST', new LambdaIntegration(updateReview));
  }
}
