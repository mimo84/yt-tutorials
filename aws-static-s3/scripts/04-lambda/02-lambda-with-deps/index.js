const AWS = require('aws-sdk');

const ddb = new AWS.DynamoDB.DocumentClient({
  endpoint: `http://${process.env.LOCALSTACK_HOSTNAME}:4566`,
});

exports.handler = async (event, context, callback) => {
  await scanMusicTable().then(data => {
    data.Items.forEach(function (item) {
      console.log(item.message);
    });
    callback(null, {
      // If success return 200, and items
      statusCode: 200,
      body: data.Items,
      headers: {
        'Access-Control-Allow-Origin': '*',
      },
    });
  }).catch((err) => {
    // If an error occurs write to the console
    console.error(err);
  });

};

const scanMusicTable = () => {
  const params = {
    TableName: 'Music',
    Limit: 10
  };
  return ddb.scan(params).promise();
};


