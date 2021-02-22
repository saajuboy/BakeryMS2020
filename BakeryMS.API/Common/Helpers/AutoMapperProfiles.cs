using System;
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
            })
            .ForMember(dest => dest.OrderDate, opt =>
            {
                opt.MapFrom(src => src.OrderDate.ToShortDateString());
            })
            .ForMember(dest => dest.DeliveryDate, opt =>
            {
                opt.MapFrom(src => src.DeliveryDate.ToShortDateString());
            });

            CreateMap<PurchaseOrderHeader, POHForDetailDto>()
            .ForMember(dest => dest.SupplierId, opt =>
            {
                opt.MapFrom(src => src.Supplier.Id);
            })
            .ForMember(dest => dest.OrderDate, opt =>
            {
                opt.MapFrom(src => src.OrderDate.DashedDate());
            })
            .ForMember(dest => dest.DeliveryDate, opt =>
            {
                opt.MapFrom(src => src.DeliveryDate.DashedDate());
            })
            .ForMember(dest => dest.PODetail, opt =>
            {
                opt.MapFrom(src => src.PurchaseOrderDetail);
            });

            CreateMap<PurchaseOrderDetail, PODForDetailDto>()
            .ForMember(dest => dest.Item, opt =>
            {
                opt.MapFrom(src => src.Item.Name);
            })
            .ForMember(dest => dest.DueDate, opt =>
            {
                opt.MapFrom(src => src.DueDate.DashedDate());
            });

            CreateMap<POHForDetailDto, PurchaseOrderHeader>()
            .ForMember(dest => dest.PurchaseOrderDetail, opt =>
            {
                opt.MapFrom(src => src.PODetail);
            })
            .ForMember(dest => dest.OrderDate, opt =>
            {
                opt.MapFrom(src => DateTime.Parse(src.OrderDate));
            })
            .ForMember(dest => dest.DeliveryDate, opt =>
            {
                opt.MapFrom(src => DateTime.Parse(src.DeliveryDate));
            });

            CreateMap<PODForDetailDto, PurchaseOrderDetail>()
            .ForPath(dest => dest.Item.Id, opt =>
            {
                opt.MapFrom(src => src.ItemId);
            })
            .ForMember(dest => dest.DueDate, opt =>
            {
                opt.MapFrom(src => DateTime.Parse(src.DueDate));
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
            CreateMap<ItemForDetailDto, Item>();

            CreateMap<ItemCategory, ItemCategoryForDetailDto>();
            CreateMap<ItemCategoryForDetailDto, ItemCategory>();

            CreateMap<Unit, UnitForDetailDto>();
            CreateMap<UnitForDetailDto, Unit>();



        }
    }
}