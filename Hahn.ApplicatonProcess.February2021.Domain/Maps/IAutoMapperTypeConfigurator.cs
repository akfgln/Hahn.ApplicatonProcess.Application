using AutoMapper;

namespace Hahn.ApplicatonProcess.February2021.Domain.Maps
{
    public interface IAutoMapperTypeConfigurator
    {
        void Configure(IMapperConfigurationExpression configuration);
    }
}