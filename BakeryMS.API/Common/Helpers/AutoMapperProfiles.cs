using System;
using AutoMapper;
using BakeryMS.API.Common.DTOs;
using BakeryMS.API.Common.DTOs.Inventory;
using BakeryMS.API.Common.DTOs.Manufacturing;
using BakeryMS.API.Common.DTOs.Master;
using BakeryMS.API.Models;
using BakeryMS.API.Models.Inventory;
using BakeryMS.API.Models.Production;
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

            //production order
            CreateMap<ProductionOrderHeader, ProdOrderHeaderForDetailDto>()
            .ForMember(dest => dest.Session, opt =>
            {
                opt.MapFrom(src => src.Session.Session);
            })
            .ForMember(dest => dest.SessionId, opt =>
            {
                opt.MapFrom(src => src.Session.Id);
            })
            .ForMember(dest => dest.User, opt =>
            {
                opt.MapFrom(src => src.User.Username);
            })
            .ForMember(dest => dest.BusinessPlace, opt =>
            {
                opt.MapFrom(src => src.BusinessPlace.Name);
            })
            .ForMember(dest => dest.BusinessPlaceId, opt =>
             {
                 opt.MapFrom(src => src.BusinessPlace.Id);
             })
            .ForMember(dest => dest.RequiredDate, opt =>
            {
                opt.MapFrom(src => src.RequiredDate.Value.DashedDate());
            })
            .ForMember(dest => dest.EnteredDate, opt =>
            {
                opt.MapFrom(src => src.EnteredDate.Value.DashedDate());
            });

            CreateMap<ProductionOrderDetail, ProdOrderDetailForDetailDto>()
            .ForMember(src => src.Item, opt =>
            {
                opt.MapFrom(dest => dest.Item.Name);
            })
            .ForMember(src => src.ItemId, opt =>
            {
                opt.MapFrom(dest => dest.ItemId);
            });

            CreateMap<ProdOrderHeaderForDetailDto, ProductionOrderHeader>()
            .ForMember(dest => dest.RequiredDate, opt =>
            {
                opt.MapFrom(src => DateTime.Parse(src.RequiredDate));
            })
            .ForMember(dest => dest.EnteredDate, opt =>
            {
                opt.MapFrom(src => DateTime.Parse(src.EnteredDate));
            });

            CreateMap<ProdOrderDetailForDetailDto, ProductionOrderDetail>();

            CreateMap<ProductionOrderHeader, ProdOrderForListDto>()
            .ForMember(dest => dest.BusinessPlaceName, opt =>
            {
                opt.MapFrom(src => src.BusinessPlace.Name);
            })
            .ForMember(dest => dest.SessionName, opt =>
            {
                opt.MapFrom(src => src.Session.Session);
            })
            .ForMember(dest => dest.UserName, opt =>
            {
                opt.MapFrom(src => src.User.Username);
            })
            .ForMember(dest => dest.EnteredDate, opt =>
            {
                opt.MapFrom(src => src.EnteredDate.Value.DashedDate());
            })
             .ForMember(dest => dest.RequiredDate, opt =>
             {
                 opt.MapFrom(src => src.RequiredDate.Value.DashedDate());
             });

            CreateMap<ProductionSession, ProdSessionForDetailDto>()
            .ForMember(dest => dest.StartTime, opt =>
            {
                opt.MapFrom(src => src.StartTime.Value.ToString());
            })
            .ForMember(dest => dest.EndTime, opt =>
            {
                opt.MapFrom(src => src.EndTime.Value.ToString());
            });

            CreateMap<BusinessPlace, BusinessPlaceForDetailDto>();

        }
    }
}