using AutoMapper;
using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Domain.Models;

namespace Hahn.ApplicatonProcess.February2021.Domain.Maps
{
    public class AssetsMap : IAutoMapperTypeConfigurator
    {
        public void Configure(IMapperConfigurationExpression configuration)
        {
            var mapAssetModelToAsset = configuration.CreateMap<AssetModel, Asset>()
                  .ForMember(dest => dest.Department, src => src.MapFrom(src => src.Department.ToString()));
            var mapAssetToAssetModel = configuration.CreateMap<Asset, AssetModel>()
                  .ForMember(dest => dest.Department, src => src.MapFrom(src => EnumHelper<Data.Departments>.Parse(src.Department)));
        }
    }
}
