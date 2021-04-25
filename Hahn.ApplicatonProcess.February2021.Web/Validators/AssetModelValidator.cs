using FluentValidation;
using Hahn.ApplicatonProcess.February2021.Domain.Models;
using System;
using System.Net.Http;

namespace Hahn.ApplicatonProcess.February2021.Web.Validators
{
    public class AssetModelValidator : AbstractValidator<AssetModel>
    {
        private readonly IHttpClientFactory _clientFactory;
        public AssetModelValidator(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;

            RuleFor(x => x.AssetName)
               .NotEmpty()
               .WithMessage("AssetName is required")
               .MinimumLength(5)
               .WithMessage("AssetName must be at least 5 Characters");

            RuleFor(x => x.Department)
                .IsInEnum()
                .WithMessage("Department must be in enum");

            RuleFor(x => x.EMailAdressOfDepartment)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress();

            RuleFor(x => x.CountryOfDepartment)
                .NotEmpty()
                .WithMessage("CountryOfDepartment is required")
                .Must(CheckCountryOfDepartment)
                .WithMessage("CountryOfDepartment must be a valid Country");

            RuleFor(x => x.PurchaseDate)
                .NotEmpty()
                .WithMessage("PurchaseDate is required")
                .LessThanOrEqualTo(DateTime.UtcNow.AddYears(1))
                .WithMessage("PurchaseDate must not be older then one year");
        }

        private bool CheckCountryOfDepartment(string arg)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
          $"https://restcountries.eu/rest/v2/name/{arg}?fullText=true");
            //request.Headers.Add("Accept", "application/vnd.github.v3+json");
            //request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var client = _clientFactory.CreateClient();
            var response = client.Send(request);
            return response.IsSuccessStatusCode;
        }
    }
}
