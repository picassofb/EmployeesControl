using System.ComponentModel.DataAnnotations;
using System.Web.Services.Configuration;

namespace EmployeeMVC.Models
{
    public class TasksModel
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string TaskDescription { get; set; }
    }
}