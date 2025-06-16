using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [StringLength(50)]
        public string Department { get; set; }
    }
}

