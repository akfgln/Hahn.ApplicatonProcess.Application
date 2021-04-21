using Hahn.ApplicatonProcess.February2021.Domain;
using Hahn.ApplicatonProcess.February2021.Domain.Maps;
using Hahn.ApplicatonProcess.February2021.Domain.Models;
using Hahn.ApplicatonProcess.February2021.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hahn.ApplicatonProcess.February2021.Web.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginRepository loginRepository;
        private readonly IAutoMapper mapper;

        public LoginController(ILoginRepository loginRepository, IAutoMapper mapper)
        {
            this.loginRepository = loginRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Authenticate method for hahn project
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Authenticate")]
        [ValidateModel]
        //[SwaggerRequestExample(typeof(LoginModel), typeof(LoginModelRequestExample))]
        public ActionResult<UserWithTokenModel> Authenticate([FromBody] LoginModel model)
        {
            var result = loginRepository.Authenticate(model.Email, model.Password);
            var resultModel = mapper.Map<UserWithTokenModel>(result);
            return StatusCode((int)HttpStatusCode.OK, resultModel);
        }

        [HttpPost("Register")]
        [ValidateModel]
        public async Task<ActionResult<UserModel>> Register([FromBody] RegisterLoginModel model)
        {
            var result = await loginRepository.Register(model);
            var resultModel = mapper.Map<UserModel>(result);
            return StatusCode((int)HttpStatusCode.Created, resultModel);
        }
    }
}
