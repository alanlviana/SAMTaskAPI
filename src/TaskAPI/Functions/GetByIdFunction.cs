using System;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using TaskAPI.DynamoDB;
using TaskAPI.Exceptions;

namespace TaskAPI.Functions
{
    public class GetByIdFunction
    {
        private const string TASK_ID = "taskId";
        private readonly ITaskRepository TaskRepository;

        public GetByIdFunction(ITaskRepository taskRepository)
        {
            this.TaskRepository = taskRepository;
        }

        public GetByIdFunction()
        {
            this.TaskRepository = new TaskRepository();
        }


        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            var taskId = apigProxyEvent.PathParameters[TASK_ID];
            Console.WriteLine($"TaskId: {taskId}");

            try{
                var task = await TaskRepository.GetByIdAsync(taskId);
                return DefaultApiGatewayResponses.Ok(task);
            }catch(ItemNotFoundException exception){
                return DefaultApiGatewayResponses.NotFound(exception.Message);
            }catch(Exception exception){
                return DefaultApiGatewayResponses.InternalServerError(exception);
            }
            

        }
    }
}