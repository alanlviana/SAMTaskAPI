using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using TaskAPI.Model;

namespace TaskAPI.ViewModel
{
    public class CreateTaskItemViewModel
    {
        public string Description{get;set;}

        public TaskItem MapTo()
        {
            return new TaskItem(){
                Description = this.Description
            };
        }
    }

    public class CreateTaskItemViewModelValidator: AbstractValidator<CreateTaskItemViewModel>{
        public CreateTaskItemViewModelValidator()
        {
            RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.Description).NotNull().NotEmpty().Length(5, 30);
        }
    }
}