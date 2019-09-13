using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc.Html;

namespace EmployeeMVC.Models
{
    public class EmployeeModel
    {
        public EmployeeModel()
        {
            PicturePath = "~/Files/Images/defaultPhoto.png";
        }

        [Key]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Name { get; set; }
        public string Position { get; set; }
        public string Office { get; set; }
        public int? Salary { get; set; }

        [DisplayName("Picture")]
        public string PicturePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase PictureUpload { get; set; }

    }
}