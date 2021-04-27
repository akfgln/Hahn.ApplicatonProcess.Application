using Hahn.ApplicatonProcess.February2021.Domain.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Hahn.ApplicatonProcess.February2021.Web.Filters.Swagger.RequestExamples
{
    public class UserModelRequestExample : IExamplesProvider<CreateUserModel>
    {
        CreateUserModel IExamplesProvider<CreateUserModel>.GetExamples()
        {
            return new CreateUserModel
            {
                Email = "asset@hahn.com",
                Password = "123456",
                FirstName = "asset",
                LastName = "hahn",
                Roles = new List<string> { "Administrator,Manager" }.ToArray()
            };
        }
    }
}
