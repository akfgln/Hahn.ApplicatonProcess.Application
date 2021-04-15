namespace Hahn.ApplicatonProcess.February2021.Domain.Models
{
    public class UpdateAssetModel
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string Department { get; set; }
        public string CountryOfDepartment { get; set; }
        public string EMailAdressOfDepartment { get; set; }
        public bool? IsBroken { get; set; }
    }
}
