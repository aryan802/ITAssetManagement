using System;
using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Models
{
    public class Vendor
    {
        [Key]
        public int VendorID { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        public string Contact { get; set; }

        [DataType(DataType.Date)]
        public DateTime ContractExpiry { get; set; }
    }
}

