using Hahn.ApplicatonProcess.February2021.Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Hahn.ApplicatonProcess.February2021.Web.Filters.Swagger.RequestExamples
{
    public class LoginModelRequestExample : IExamplesProvider<LoginModel>
    {
        LoginModel IExamplesProvider<LoginModel>.GetExamples()
        {
            return new LoginModel { Email = "admin@hahn.com", Password = "admin" };
        }
    }
}
