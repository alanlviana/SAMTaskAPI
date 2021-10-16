using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using TaskAPI.DynamoDB;
using TaskAPI.Exceptions;
using TaskAPI.Model;
using TaskAPI.ViewModel;

namespace TaskAPI.Functions
{
    public class PutFunction
    {
        private const string TASK_ID = "taskId";

        private readonly TaskRepository TaskRepository;
        public PutFunction()
        {
            TaskRepository = new TaskRepository();
        }


        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context){

            Console.WriteLine("Task received");
            var taskId = apigProxyEvent.PathParameters[TASK_ID];
            Console.WriteLine($"TaskId: {taskId}");

            var updateTask = JsonConvert.DeserializeObject<UpdateTaskItemViewModel>(apigProxyEvent.Body);
            var taskValidationResult = new UpdateTaskItemViewModelValidator().Validate(updateTask);

            if (!taskValidationResult.IsValid){
                return DefaultApiGatewayResponses.BadRequest(taskValidationResult);
            }

            try{
                var taskToUpdate = await TaskRepository.GetByIdAsync(taskId);
                updateTask.MapTo(taskToUpdate);
                await TaskRepository.AddOrUpdateAsync(taskToUpdate);
                return DefaultApiGatewayResponses.Ok(taskToUpdate);
            }catch(ItemNotFoundException exception){
                return DefaultApiGatewayResponses.NotFound(exception.Message);
            }catch(Exception exception){
                return DefaultApiGatewayResponses.InternalError(exception);
            }

        }
    }
}