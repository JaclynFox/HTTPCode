using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ReturnCat
{

    public class Function
    {
        public static HttpClient client = new HttpClient();
        public static AmazonDynamoDBClient dynamo = new AmazonDynamoDBClient();
        public string table = "HTTPCats";
        public async Task<string> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            string code = string.Empty;
            input.QueryStringParameters.TryGetValue("code", out code);
            Dictionary<string, AttributeValue> gets = new Dictionary<string, AttributeValue>();
            gets.Add("code", new AttributeValue { N = code });
            GetItemResponse res = await dynamo.GetItemAsync(table, gets);
            AttributeValue img = new AttributeValue();
            if (res.Item.Count == 0)
            {
                HttpRequestMessage req = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    Content = new StringContent("{\"code\":\"" + code + "\"}", Encoding.UTF8, "application/json"),
                    RequestUri = new Uri("https://hofzb2bu62.execute-api.us-west-2.amazonaws.com/gethttpcode")
                };
                await client.SendAsync(req);
                res = await dynamo.GetItemAsync(table, gets);
            }
            return res.Item["image"].S;
        }
    }
}
