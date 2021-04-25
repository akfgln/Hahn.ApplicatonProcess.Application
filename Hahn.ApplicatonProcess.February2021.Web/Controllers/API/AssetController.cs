using AutoQueryable.AspNetCore.Filter.FilterAttributes;
using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Domain;
using Hahn.ApplicatonProcess.February2021.Domain.Maps;
using Hahn.ApplicatonProcess.February2021.Domain.Models;
using Hahn.ApplicatonProcess.February2021.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AssetController : Controller
    {
        private readonly IAssetRepository assetRepository;
        private readonly IHttpClientFactory clientFactory;
        private readonly IAutoMapper mapper;

        public AssetController(
            IAssetRepository assetRepository, 
            IHttpClientFactory clientFactory, 
            IAutoMapper mapper)
        {
            this.assetRepository = assetRepository;
            this.clientFactory = clientFactory;
            this.mapper = mapper;
        }

        [HttpGet]
        [AutoQueryable(DefaultToTake = 50)]
        public ActionResult<IQueryable<AssetModel>> Get()
        {
            var result = assetRepository.Get();
            var models = mapper.Map<Asset, AssetModel>(result);
            return Ok(models);
        }

        [HttpGet("{id}")]
        public ActionResult<AssetModel> Get(int id)
        {
            var item = assetRepository.Get(id);
            var model = mapper.Map<AssetModel>(item);
            return Ok(model);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<AssetModel>> Post([FromBody] AssetModel requestModel)
        {
            var item = await assetRepository.Create(requestModel);
            var model = mapper.Map<AssetModel>(item);
            return StatusCode((int)HttpStatusCode.Created, model);
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<ActionResult<AssetModel>> Put(int id, [FromBody] AssetModel requestModel)
        {
            var item = await assetRepository.Update(id, requestModel);
            var model = mapper.Map<AssetModel>(item);
            return StatusCode((int)HttpStatusCode.Accepted, model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await assetRepository.Delete(id);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [HttpGet("GetCountries")]
        public ActionResult<IList<CountryModel>> GetCountries()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
          $"https://restcountries.eu/rest/v2/all");

            var client = this.clientFactory.CreateClient();
            var response = client.Send(request);
            var responseString =  response.Content.ReadAsStringAsync().Result;
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<CountryModel>>(responseString);

            return Ok(model);
        }
    }
}
