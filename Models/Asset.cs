using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Models
{
    public class Asset
    {
        public int AssetID { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Type { get; set; }

        [Required, StringLength(50)]
        public string Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime WarrantyExpiry { get; set; }

        // Foreign‐key to Employee (nullable)
        public int? AssignedTo { get; set; }


        [ForeignKey("AssignedTo")]
        public Employee? AssignedEmployee { get; set; }

    }
}

