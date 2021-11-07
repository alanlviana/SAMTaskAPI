using System;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using TaskAPI.DynamoDB;
using TaskAPI.Exceptions;

namespace TaskAPI.Functions
{
    public class DeleteByIdFunction
    {
        private const string TASK_ID = "taskId";
        ITaskRepository TaskRepository = new TaskRepository();


        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            var taskId = apigProxyEvent.PathParameters[TASK_ID];
            Console.WriteLine($"TaskId: {taskId}");

            try{
                var task = await TaskRepository.GetByIdAsync(taskId);
                await TaskRepository.DeleteByIdAsync(taskId);
                Console.WriteLine($"TaskItem ({taskId}) was deleted.");
                return DefaultApiGatewayResponses.Ok(new{ Message= $"Task ({taskId}) was deleted."});
            }catch(ItemNotFoundException exception){
                return DefaultApiGatewayResponses.NotFound(exception.Message);
            }catch(Exception exception){
                return DefaultApiGatewayResponses.InternalServerError(exception);
            }
            

        }
    }
}