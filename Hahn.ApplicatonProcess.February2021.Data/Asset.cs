using System;

namespace Hahn.ApplicatonProcess.February2021.Data
{
    public class Asset
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string Department { get; set; }
        public string CountryOfDepartment { get; set; }
        public string EMailAdressOfDepartment { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool? IsBroken { get; set; }
        public bool IsDeleted { get; set; }
    }
}
