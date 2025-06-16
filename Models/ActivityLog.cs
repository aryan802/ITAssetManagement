using System;
using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }

        [Required]
        public string Action { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public string? UserName { get; set; }

        public int? AssetID { get; set; }
        public string? Details { get; set; }
    }
}


