using System.Collections.Generic;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System;
using System.Threading.Tasks;
using TaskAPI.DynamoDB;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TaskAPI.Functions
{

    public class GetFunction
    {
        ITaskRepository TaskRepository = new TaskRepository();
        
        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            try{
                var taskList = await TaskRepository.GetAllAsync();
                var body = new Dictionary<string, object>
                {
                    { "tasks", taskList }
                };
                return DefaultApiGatewayResponses.Ok(body);
            }catch(Exception exception){
                return DefaultApiGatewayResponses.InternalServerError(exception);
            }
            

        }
    }
}
