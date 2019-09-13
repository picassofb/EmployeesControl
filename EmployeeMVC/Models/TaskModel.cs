using System.ComponentModel.DataAnnotations;
using System.Web.Services.Configuration;

namespace EmployeeMVC.Models
{
    public class TaskModel
    {
        [Key]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string TaskDescription { get; set; }
    }
}