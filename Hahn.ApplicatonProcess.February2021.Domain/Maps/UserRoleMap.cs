using AutoMapper;
using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Domain.Models;

namespace Hahn.ApplicatonProcess.February2021.Domain.Maps
{
    public class UserRoleMap : IAutoMapperTypeConfigurator
    {
        public void Configure(IMapperConfigurationExpression configuration)
        {
            var mapCreateUserRoleToUserRole = configuration.CreateMap<UserRoleModel, UserRoles>();
            var mapFromTblUserRolesToUserROleModel = configuration.CreateMap<UserRoles, UserRoleModel>()
                .IncludeMembers(u => u.Role, u => u.User)
                .ForMember(dest => dest.RoleName, src => src.MapFrom(src => src.Role.DefaultRoleName))
                .ForMember(dest => dest.UserName, src => src.MapFrom(src => src.User.FirstName + " " + src.User.LastName));
            var map1 = configuration.CreateMap<Roles, UserRoleModel>(MemberList.None);
            var map2 = configuration.CreateMap<Users, UserRoleModel>(MemberList.None);
        }
    }
}
