using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace VAT_TODOLIST.Models
{
    public class VATTodoModel
    {       
        public int Id { get; set; }
        [Required(ErrorMessage ="Please Fill in the Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Fill in the Task Name")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email address.")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage ="Please fill Email Address")]
        public string TaskName { get; set; }
        [Required(ErrorMessage ="Please Select Proper date")]
        public DateTime TaskDate { get; set; }
        [Required(ErrorMessage ="Please Fill in the Task Priority")]
        public string Priority { get; set; }        
             
    }

    public class VATTodoViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Fill in the Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Fill in the Task Name")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email address.")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Please fill Email Address")]
        public string TaskName { get; set; }
        [Required(ErrorMessage = "Please Select Proper date")]
        public DateTime TaskDate { get; set; }
        [Required(ErrorMessage = "Please Fill in the Task Priority")]
        public string Priority { get; set; }
        public string HiddenId { get; set; }
    }

}
