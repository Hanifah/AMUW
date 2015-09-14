using AMUW.Data.Model;
using AMUW.ViewModels;
using AutoMapper;

namespace AMUW.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<UserViewModel, User>();
            Mapper.CreateMap<VMUser, VirtualMachineViewModel>();
        }
    }
}