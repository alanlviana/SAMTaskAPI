using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskAPI.Model
{
    public class TaskItem
    {
        [Required(ErrorMessage = "Id is required")]
        public string Id{get;set;}
        [Required (ErrorMessage = "Description is required")]
        [StringLength(30, ErrorMessage = "Description must have a maximum of 30 characters. ")]
        [MinLength(5, ErrorMessage = "Description must have a minimum of 5 characters.")]
        public string Description{get;set;}

        public List<ValidationResult> Validate(){
            var context = new ValidationContext(this);
            var validationResult = new List<ValidationResult>();
            Validator.TryValidateObject(this, context, validationResult, true);
            return validationResult;
        }
    }
}