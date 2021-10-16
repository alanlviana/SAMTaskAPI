using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using TaskAPI.Model;

namespace TaskAPI.ViewModel
{
    public class UpdateTaskItemViewModel
    {
        public string Description{get;set;}
        public bool? Done{get;set;}

        internal TaskItem MapTo(TaskItem taskToUpdate)
        {
            if (taskToUpdate == null){
                throw new ArgumentNullException(nameof(taskToUpdate));
            }

            taskToUpdate.Description = Description;
            taskToUpdate.Done = Done.Value;
            return taskToUpdate;
        }
    }

    public class UpdateTaskItemViewModelValidator: AbstractValidator<UpdateTaskItemViewModel>{
        public UpdateTaskItemViewModelValidator()
        {
            RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.Description).NotNull().NotEmpty().Length(5, 30);
            RuleFor(x => x.Done).NotEmpty().WithMessage("Done is required.");
        }
    }
}