using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

using GetCats;

namespace GetCats.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async void TestToUpperFunction()
        {

            // Invoke the lambda function and confirm the string was upper cased.
            var function = new Function();
            var context = new TestLambdaContext();
            APIGatewayProxyRequest a = new APIGatewayProxyRequest();
            a.Body = "{\"code\":\"599\"}";
            var upperCase = await function.FunctionHandler(a, context);

            Assert.Equal("OK", upperCase);
        }
    }
}
