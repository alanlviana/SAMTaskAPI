using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace TaskAPI.Model
{
    public class TaskItem
    {
        public string Id{get;set;}
        public string Description{get;set;}
        public bool Done{get;set;}
    }


}