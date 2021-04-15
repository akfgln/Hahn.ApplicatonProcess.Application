using System;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models
{
    public class CreateAssetModel
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public Departments Department { get; set; }
        public string CountryOfDepartment { get; set; }
        public string EMailAdressOfDepartment { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool? IsBroken { get; set; }
    }
}
