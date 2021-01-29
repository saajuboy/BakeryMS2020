using AutoMapper;
using BakeryMS.API.Common.DTOs;
using BakeryMS.API.Common.DTOs.Inventory;
using BakeryMS.API.Common.DTOs.Master;
using BakeryMS.API.Models;
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
            .ForMember(dest => dest.SupplierName, opt =>
            {
                opt.MapFrom(src => src.Supplier.Name);
            });
            CreateMap<PurchaseOrderHeader, POHForDetailDto>()
            .ForMember(dest => dest.SupplierId, opt =>
            {
                opt.MapFrom(src => src.Supplier.Id);
            }).ForMember(dest => dest.PODetail, opt =>
            {
                opt.MapFrom(src => src.PurchaseOrderDetail);
            });
            CreateMap<PurchaseOrderDetail, PODForDetailDto>()
            .ForMember(dest => dest.Item, opt =>
            {
                opt.MapFrom(src => src.Item.Name);
            });

            CreateMap<POHForDetailDto, PurchaseOrderHeader>()
            .ForMember(dest => dest.PurchaseOrderDetail, opt =>
            {
                opt.MapFrom(src => src.PODetail);
            });

            CreateMap<PODForDetailDto, PurchaseOrderDetail>()
            .ForPath(dest => dest.Item.Id, opt =>
            {
                opt.MapFrom(src => src.ItemId);
            });

            CreateMap<Supplier, SupplierDto>();

            CreateMap<SupplierDto, Supplier>();

            CreateMap<Item, ItemForListDto>()
            .ForMember(dest => dest.ItemCategory, opt =>
            {
                opt.MapFrom(src => src.ItemCategory.Description);
            })
            .ForMember(dest => dest.Unit, opt =>
            {
                opt.MapFrom(src => src.Unit.Description);
            });

            CreateMap<Item, ItemForDetailDto>();
            CreateMap<ItemCategory, ItemCategoryForDetailDto>();
            CreateMap<Unit, UnitForDetailDto>();


        }
    }
}