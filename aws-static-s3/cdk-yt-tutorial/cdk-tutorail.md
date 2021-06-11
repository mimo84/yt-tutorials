# Install the dependencies

aws-cli, aws-cdk-cli
$ npm install -g aws-cdk-local aws-cdk

# Configure Localstack

Ensure that serverless,sns,sqs are present.

# Create the project

cdk init sample-app --language typescript

# Present the structure of the project

# CDK Synth

Perform in the console:

$ cdk synth

It is going to generate a template for CloudFormation.

If we want we can save the contents to a file:

$ cdk synth CdkTutorial --profile personal > CdkTutorial.template.yml

# Bootstrapping the environment

$ cdklocal bootstrap

## and deployment:

$ cdklocal deploy

## List the cloudformation stacks:

$ aws cloudformation list-stacks --endpoint-url http://localhost:4566

## And the list of resources we deployed

aws cloudformation list-stack-resources --stack-name CdkYtTutorialStack --endpoint-url http://localhost:4566

## Let's set up a basic API Gateway that calls an AWS Lambda and responds to the user

- Start by cleaning up code and remove the content in the lib/class file

- we can check what changed since last deployment and the current stack:

$ cdklocal diff

# Add AWS Lambda library

$ npm install @aws-cdk/aws-lambda

If you have problems to install at this stage ensure that all the libraries are at the same version. At this time for example 107

Then we can simply add a lamdba function

- Create a folder root/lambda/hello.js
- Add some code to log the event and return a 200 OK response

```
exports.handler = async function (event) {
  console.log("request:", JSON.stringify(event, undefined, 2));
  return {
    statusCode: 200,
    headers: { "Content-Type": "text/plain" },
    body: `Hello, CDK! You've hit ${event.path}\n`
  };
};
```

And deploy:

$ cdnlocal diff

$ cdnlocal deploy

# Let's find out the name of the function:

$ aws --endpoint-url=http://localhost:4566 lambda list-functions

Grab the name of the function we just deployed:

CdkTutorialStack-lambda-774e243e

aws lambda invoke \
 --cli-binary-format raw-in-base64-out \
 --function-name CdkYtTutorialStack-lambda-f2423e2d \
 --invocation-type RequestResponse \
 --no-sign-request \
 --payload file://apigateway-aws-proxy.json \
 --endpoint http://localhost:4566 \
 output.json

And we should get an `output.json` file with the contents.

# Add AWS API Gateway

Install the library:

$ npm i @aws-cdk/aws-apigateway

Let's connect ApiGateway with our handler. In `lib/cdk-tutorial-stack.ts` we add:

```
new apigw.LambdaRestApi(this, 'Endpoint', {
  handler: hello
});
```

which is basically defining a new rest api

and now running:

$ cdklocal diff

We're going to see that we added a few more resources to connect all of it together.

Finally deploy again:

$ cdklocal deploy

At the end of the command we are going to get an output that looks something like this:

```
Outputs:
CdkTutorialStack.Endpoint8024A810 = https://0dghlfql09.execute-api.us-east-1.localhost/prod/

Stack ARN:
arn:aws:cloudformation:us-east-1:000000000000:stack/CdkTutorialStack/baac9c3b
```

Of course we can't hit that URL like we would do normally, so let's break down the meaning of it.

First of all we need to find the ID of our rest api.

Execute:

$ aws apigateway get-rest-apis --endpoint-url=http://localhost:4566

Is going to output something like this:

```
{
    "items": [
        {
            "id": "0dghlfql09",
            "name": "Endpoint",
            "createdDate": "2021-06-10T14:26:24+10:00",
            "apiKeySource": "HEADER",
            "endpointConfiguration": {
                "types": [
                    "EDGE"
                ]
            },
            "tags": {},
            "disableExecuteApiEndpoint": false
        }
    ]
}
```

The name in the `items` array is going to match the name of the function that we defined earlier. So the name `Endpoint` is what we are looking for and the `ID` of the resource is the same.
From that blob we can get the id which in this case is `0dghlfql09` and matches with the first part of our URL.

The next bit is to find out the parent-ID:

$ aws apigateway get-resources --rest-api-id 0dghlfql09 --endpoint-url=http://localhost:4567

The output is going to look like this:

```
{
    "items": [
        {
            "id": "7m2n8eq8n8",
            "path": "/",
            "resourceMethods": {
                "ANY": {
                    "httpMethod": "ANY",
                    "authorizationType": "NONE",
                    "apiKeyRequired": false,
                    "methodIntegration": {
                        "type": "AWS_PROXY",
                        "httpMethod": "POST",
                        "uri": "arn:aws:apigateway:us-east-1:lambda:path/2015-03-31/functions/arn:aws:lambda:us-east-1:000000000000:function:CdkTutorialStack-lambda-774e243e/invocations",
                        "requestParameters": {},
                        "passthroughBehavior": "WHEN_NO_MATCH",
                        "cacheNamespace": "3421a1a9",
                        "cacheKeyParameters": [],
                        "integrationResponses": {
                            "200": {
                                "statusCode": 200,
                                "responseTemplates": {
                                    "application/json": null
                                }
                            }
                        }
                    }
                }
            }
        },
        {
            "id": "8u498jfrj3",
            "parentId": "7m2n8eq8n8",
            "pathPart": "{proxy+}",
            "path": "/{proxy+}",
            "resourceMethods": {
                "ANY": {
                    "httpMethod": "ANY",
                    "authorizationType": "NONE",
                    "apiKeyRequired": false,
                    "methodIntegration": {
                        "type": "AWS_PROXY",
                        "httpMethod": "POST",
                        "uri": "arn:aws:apigateway:us-east-1:lambda:path/2015-03-31/functions/arn:aws:lambda:us-east-1:000000000000:function:CdkTutorialStack-lambda-774e243e/invocations",
                        "requestParameters": {},
                        "passthroughBehavior": "WHEN_NO_MATCH",
                        "cacheNamespace": "cea45543",
                        "cacheKeyParameters": [],
                        "integrationResponses": {
                            "200": {
                                "statusCode": 200,
                                "responseTemplates": {
                                    "application/json": null
                                }
                            }
                        }
                    }
                }
            }
        }
    ]
}
```

Which means that regardless of the method we are using (POST | GET | PUT | DELETE | PATCH etc) it is going to return the same thing.

Let's hit the API:

$ curl http://localhost:4566/restapis/qqybe4q5ce/execute-api/_user_request_/

Response:

```
Hello, CDK! You've hit /
```

$ curl http://localhost:4566/restapis/0dghlfql09/execute-api/_user_request_/test/

Response:

```
Hello, CDK! You've hit /test
```

Now we can go back to our bucket sample website and fetch the data from the backend!
