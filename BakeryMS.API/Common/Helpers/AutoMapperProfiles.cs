using AutoMapper;
using BakeryMS.API.Common.DTOs;
using BakeryMS.API.Common.DTOs.Inventory;
using BakeryMS.API.Models.Inventory;
using BakeryMS.API.Models.Profile;

namespace BakeryMS.API.Common.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForRegisterDto, User>();
            CreateMap<User, UserForDetailDto>();
            CreateMap<PurchaseOrderHeader, POForListDto>()
            .ForMember(dest=> dest.SupplierName,opt=>{
                opt.MapFrom(src=>src.Supplier.Name);
            });
            CreateMap<PurchaseOrderHeader, POHForDetailDto>()
            .ForMember(dest=> dest.Supplier,opt=>{
                opt.MapFrom(src=>src.Supplier.Name);
            });
            CreateMap<PurchaseOrderDetail, PODForDetailDto>()
            .ForMember(dest => dest.Item, opt =>{
                opt.MapFrom(src => src.Item.Name);
            });

        }
    }
}