using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Models
{
    public class AssetVendor
    {
        [Key, Column(Order = 0)]
        public int AssetID { get; set; }

        [Key, Column(Order = 1)]
        public int VendorID { get; set; }

        // Navigation properties (optional)
        public Asset Asset { get; set; }
        public Vendor Vendor { get; set; }
    }
}

