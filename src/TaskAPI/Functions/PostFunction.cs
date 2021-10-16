using System;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using TaskAPI.Model;
using Newtonsoft.Json;
using TaskAPI.DynamoDB;
using TaskAPI.ViewModel;
using System.IO;

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
            var createTask = JsonConvert.DeserializeObject<CreateTaskItemViewModel>(apiaProxyEvent.Body);
            var taskValidationResult = new CreateTaskItemViewModelValidator().Validate(createTask);

            if (!taskValidationResult.IsValid){
                return DefaultApiGatewayResponses.BadRequest(taskValidationResult);
            }

            var task = createTask.MapTo();
            task.Id = Guid.NewGuid().ToString();
            task.Done = false;

            try{
                await TaskRepository.AddOrUpdateAsync(task);
            }catch(Exception exception){
                return DefaultApiGatewayResponses.InternalError(exception);
            }

            var location = Path.Combine(apiaProxyEvent.Headers["Host"], "task", task.Id);
            return DefaultApiGatewayResponses.Created(task, location);

        }
    }
}