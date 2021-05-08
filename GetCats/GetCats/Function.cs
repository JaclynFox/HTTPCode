using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GetCats
{
    public class Code
    {
        public string code;
    }
    public class Function
    {
        public static HttpClient client = new HttpClient();
        public static AmazonDynamoDBClient dynamo = new AmazonDynamoDBClient();
        public string table = "HTTPCats";
        public async Task<string> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            Code code = JsonConvert.DeserializeObject<Code>(input.Body);
            string urlstring = "https://http.cat/" + code.code;
            HttpRequestMessage req = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                Content = null,
                RequestUri = new Uri(urlstring)
            };
            HttpResponseMessage res = await client.SendAsync(req);
            Dictionary<string, AttributeValue> puts = new Dictionary<string, AttributeValue>();
            puts["code"] = new AttributeValue { N = code.code };
            puts["image"] = new AttributeValue { S = Convert.ToBase64String(await res.Content.ReadAsByteArrayAsync()) };        
            PutItemResponse putres = await dynamo.PutItemAsync(table, puts);
            return putres.HttpStatusCode.ToString();
        }
    }
}
