using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using TaskAPI.Exceptions;

namespace TaskAPI
{
    public class DefaultApiGatewayResponses
    {
        public static APIGatewayProxyResponse BadRequest(FluentValidation.Results.ValidationResult validationResult){

            var errors = validationResult.Errors.Select(e => new{Field = e.PropertyName, Message=e.ErrorMessage});
            Console.WriteLine($"A bad request status code was returned.");
            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(errors),
                StatusCode = 400,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }

        public static APIGatewayProxyResponse InternalServerError(Exception e){
            Console.WriteLine($"An internal error status code was returned. Exception Message: {e.Message}");
            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(new {message=e.Message}),
                StatusCode = 500,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }

        public static APIGatewayProxyResponse NotFound(string message)
        {
            Console.WriteLine($"A not found status code was returned.");
            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(new {message = message}),
                StatusCode = 404,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }

        public static APIGatewayProxyResponse Created(Object objectCreated, String location){
            Console.WriteLine($"A created status code was returned.");
            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(objectCreated),
                StatusCode = 201,
                Headers = new Dictionary<string, string> {
                     { "Content-Type", "application/json" },
                     { "Location", location },
                }
            };
        }

        public static APIGatewayProxyResponse Ok(Object body){
            Console.WriteLine($"An OK status code was returned.");
            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(body),
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }


    }
}