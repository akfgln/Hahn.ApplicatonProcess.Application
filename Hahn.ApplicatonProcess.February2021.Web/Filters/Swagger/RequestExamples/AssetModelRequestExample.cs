using Hahn.ApplicatonProcess.February2021.Domain.Models;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Hahn.ApplicatonProcess.February2021.Web.Filters.Swagger.RequestExamples
{
    public class AssetModelRequestExample : IExamplesProvider<AssetModel>
    {
        AssetModel IExamplesProvider<AssetModel>.GetExamples()
        {
            return new AssetModel { 
                EMailAdressOfDepartment = "asset@hahn.com", 
                AssetName = "admin", 
                CountryOfDepartment = "Germany", 
                Department = Departments.Store3, 
                IsBroken = null, 
                PurchaseDate = DateTime.UtcNow 
            };
        }
    }
}
