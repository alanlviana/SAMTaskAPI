using System;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using TaskAPI.Model;
using Newtonsoft.Json;
using TaskAPI.DynamoDB;

namespace TaskAPI.Functions
{
    public class PostFunction
    {
        private readonly TaskRepository TaskRepository;

        public PostFunction()
        {
            TaskRepository = new TaskRepository();
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apiaProxyEvent, ILambdaContext context){

            Console.WriteLine("Task received");
            var task = JsonConvert.DeserializeObject<TaskItem>(apiaProxyEvent.Body);
            task.Id = Guid.NewGuid().ToString();
            var validationResult =  task.Validate();

            if (validationResult.Count > 0){
                return DefaultApiGatewayResponses.BadRequest(validationResult);
            }

            try{
                await TaskRepository.Add(task);
            }catch(Exception exception){
                return DefaultApiGatewayResponses.InternalError(exception);
            }
            
            return DefaultApiGatewayResponses.Created(task);

        }
    }
}