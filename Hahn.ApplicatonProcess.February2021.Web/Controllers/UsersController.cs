using AutoQueryable.AspNetCore.Filter.FilterAttributes;
using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Domain;
using Hahn.ApplicatonProcess.February2021.Domain.Common;
using Hahn.ApplicatonProcess.February2021.Domain.Maps;
using Hahn.ApplicatonProcess.February2021.Domain.Models;
using Hahn.ApplicatonProcess.February2021.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Web.Controllers
{

    [Route("api/[controller]")]
    [Authorize(Roles = SystemRoles.AdministratorOrManager)]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IAutoMapper mapper;

        public UsersController(IUserRepository userRepository, IAutoMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [AutoQueryable(DefaultToTake = 50)]
        public ActionResult<IQueryable<UserModel>> Get()
        {
            var result = userRepository.Get();
            var models = mapper.Map<Users, UserModel>(result);
            return Ok(models);
        }

        [HttpGet("{id}")]
        public ActionResult<UserModel> Get(int id)
        {
            var item = userRepository.Get(id);
            var model = mapper.Map<UserModel>(item);
            return Ok(model);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<UserModel>> Post([FromBody] CreateUpdateUserModel requestModel)
        {
            var item = await userRepository.Create(requestModel);
            var model = mapper.Map<UserModel>(item);
            return StatusCode((int)HttpStatusCode.Created, model);
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<ActionResult<UserModel>> Put(int id, [FromBody] CreateUpdateUserModel requestModel)
        {
            var item = await userRepository.Update(id, requestModel);
            var model = mapper.Map<UserModel>(item);
            return StatusCode((int)HttpStatusCode.Accepted, model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await userRepository.Delete(id);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
