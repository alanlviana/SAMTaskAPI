using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using TaskAPI.Exceptions;
using TaskAPI.Model;

namespace TaskAPI.DynamoDB
{
    public class TaskRepository
    {
        private const string TASK_ID = "id";
        private const string TASK_DESCRIPTION = "description";
        private const string TASK_DONE = "done";
        private readonly AmazonDynamoDBClient DDB;
        private readonly String TableName;
        private readonly String Region;
        
        public TaskRepository(){
            
            var AwsEnv = Environment.GetEnvironmentVariable("AWS_ENV");
            Console.WriteLine($"Running on environment: {AwsEnv}");
            
            TableName = Environment.GetEnvironmentVariable("TABLE_NAME");
            Console.WriteLine($"Table name: {TableName}");

            Region = Environment.GetEnvironmentVariable("REGION_NAME");
            Console.WriteLine($"Region: {Region}");


            if (AwsEnv == "AWS_SAM_LOCAL"){
                var clientConfig = new AmazonDynamoDBConfig { ServiceURL = "http://host.docker.internal:4566" };
                DDB =  new AmazonDynamoDBClient(clientConfig);
            }else{
                var RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(Region);
                DDB =  new AmazonDynamoDBClient(RegionEndpoint);
            }
        }

        public async Task<TaskItem> GetById(string taskId)
        {
            var response = await DDB.GetItemAsync(TableName, new Dictionary<string, AttributeValue>(){
                {TASK_ID, new AttributeValue() {S = taskId}}
            });

            Console.WriteLine($"GetItemAsync return a {response.HttpStatusCode} status code");
            
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK){
                Console.WriteLine($"GetItemAsync return a not OK status code: {response.HttpStatusCode}");
                Console.WriteLine($"GetItemAsync response Metadata: {JsonConvert.SerializeObject(response.ResponseMetadata)}");
                throw new Exception($"An internal server error occurred.");
            }

            if (!response.Item.ContainsKey(TASK_ID)){
                throw new ItemNotFoundException("Task not found");
            }

            return new TaskItem(){
                Id = response.Item[TASK_ID].S,
                Description = response.Item[TASK_DESCRIPTION].S,
                Done = response.Item[TASK_DONE].BOOL
            };
        }

        public async Task Add(TaskItem task){
            var item = new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>();
            item[TASK_ID] = new AttributeValue{ S = task.Id.ToString() };
            item[TASK_DESCRIPTION] = new AttributeValue{ S = task.Description };
            item[TASK_DONE] = new AttributeValue{ BOOL = task.Done };
            var response = await DDB.PutItemAsync(TableName, item);
            
            Console.WriteLine($"PutItemAsync return a {response.HttpStatusCode} status code");

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK){
                Console.WriteLine($"PutItemAsync return a not OK status code: {response.HttpStatusCode}");
                Console.WriteLine($"PutItemAsync response Metadata: {JsonConvert.SerializeObject(response.ResponseMetadata)}");
                throw new Exception($"An internal server error occurred.");
            }
        }

        public async Task<List<TaskItem>> GetAll(){
            
            var response = await DDB.ScanAsync(TableName, new List<String>{ TASK_ID, TASK_DESCRIPTION });

            Console.WriteLine($"ScanAsync return a {response.HttpStatusCode} status code");

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK){
                Console.WriteLine($"ScanAsync return a not OK status code: {response.HttpStatusCode}");
                Console.WriteLine($"Response Metadata: {JsonConvert.SerializeObject(response.ResponseMetadata)}");
                throw new Exception($"An internal server error occurred.");
            }

            var taskList = new List<TaskItem>();
            foreach(var item in response.Items){
                var taskItem = new TaskItem();
                taskItem.Id = item[TASK_ID].S;
                taskItem.Description = item[TASK_DESCRIPTION].S;
                taskItem.Done = item[TASK_DONE].BOOL;
                taskList.Add(taskItem);
            }

            return taskList;
        }
    }
}