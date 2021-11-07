
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Moq;
using Newtonsoft.Json;
using TaskAPI.DynamoDB;
using TaskAPI.Exceptions;
using TaskAPI.Functions;
using TaskAPI.Model;
using Xunit;

namespace TaskAPI.Test.Functions
{
    public class GetByIdFunctionTest
    {
        [Fact]
        public async void GetByIdFunction_OnNotItemNotFoundException_ReturnsNotFoundStatusCode(){
            TestLambdaContext context;
            APIGatewayProxyRequest request = new APIGatewayProxyRequest();
            request.PathParameters = new Dictionary<string, string>(){{"taskId", "123"}};
            APIGatewayProxyResponse response;

            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync("123")).Throws(new ItemNotFoundException("task not found"));

            GetByIdFunction functions = new GetByIdFunction(repositoryMock.Object);


            context = new TestLambdaContext();
            response = await functions.FunctionHandler(request, context);
            Assert.Equal(404, response.StatusCode);
        }

        [Fact]
        public async void GetByIdFunction_OnException_ReturnsInternalServerErrorStatusCode()
        {
            TestLambdaContext context;
            APIGatewayProxyRequest request = new APIGatewayProxyRequest();
            request.PathParameters = new Dictionary<string, string>() { { "taskId", "123" } };
            APIGatewayProxyResponse response;

            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync("123")).Throws(new Exception("task not found"));

            GetByIdFunction functions = new GetByIdFunction(repositoryMock.Object);


            context = new TestLambdaContext();
            response = await functions.FunctionHandler(request, context);
            Assert.Equal(500, response.StatusCode);
        }

        [Fact]
        public async void GetByIdFunction_RepositoryReturnsAnItem_ReturnsOkStatusCodeAndAnItemOnBody()
        {
            TestLambdaContext context;
            APIGatewayProxyRequest request = new APIGatewayProxyRequest();
            request.PathParameters = new Dictionary<string, string>() { { "taskId", "123" } };
            APIGatewayProxyResponse response;

            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync("123")).Returns(Task.FromResult(new TaskItem() { Id = "123", Description = "A new task", Done = false }));

            GetByIdFunction functions = new GetByIdFunction(repositoryMock.Object);


            context = new TestLambdaContext();
            response = await functions.FunctionHandler(request, context);
            Assert.Equal(200, response.StatusCode);

            var taskReturned = JsonConvert.DeserializeObject<TaskItem>(response.Body);
            Assert.Equal("123", taskReturned.Id);
            Assert.Equal("A new task", taskReturned.Description);
            Assert.False(taskReturned.Done);
        }
    }

    

}