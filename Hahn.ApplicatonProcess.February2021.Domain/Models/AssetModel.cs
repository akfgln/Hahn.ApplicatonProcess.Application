using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models
{
    public class AssetModel
    {
        public int Id { get; set; }
        public string AssetName { get; set; }

        [EnumDataType(typeof(Departments))]
        [JsonConverter(typeof(StringEnumConverter))]
        public Departments Department { get; set; }
        public string CountryOfDepartment { get; set; }
        public string EMailAdressOfDepartment { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsBroken { get; set; }
    }
}
