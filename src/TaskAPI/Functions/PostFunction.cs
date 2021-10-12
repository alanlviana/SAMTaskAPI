using System;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using TaskAPI.Model;
using Newtonsoft.Json;
using TaskAPI.DynamoDB;
using TaskAPI.ViewModel;

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

            try{
                await TaskRepository.Add(task);
            }catch(Exception exception){
                return DefaultApiGatewayResponses.InternalError(exception);
            }
            
            return DefaultApiGatewayResponses.Created(task);

        }
    }
}