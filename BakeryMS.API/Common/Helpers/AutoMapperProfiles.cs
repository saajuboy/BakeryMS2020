using System;
using AutoMapper;
using BakeryMS.API.Common.DTOs;
using BakeryMS.API.Common.DTOs.HumanResource;
using BakeryMS.API.Common.DTOs.Inventory;
using BakeryMS.API.Common.DTOs.Manufacturing;
using BakeryMS.API.Common.DTOs.Master;
using BakeryMS.API.Models;
using BakeryMS.API.Models.HumanResource;
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

            //Ingredients
            CreateMap<IngredientHeader, IngredientHeaderForDetailDto>()
            .ForMember(dest => dest.ItemName, opt =>
            {
                opt.MapFrom(src => src.Item.Name);
            })
            .ForMember(dest => dest.IngredientDetails, opt =>
            {
                opt.MapFrom(src => src.IngredientsDetail);
            });
            CreateMap<IngredientDetail, IngredientDetailForDetailDto>()
            .ForMember(dest => dest.ItemName, opt =>
            {
                opt.MapFrom(src => src.Item.Name);
            });

            CreateMap<IngredientHeaderForDetailDto, IngredientHeader>()
            .ForMember(dest => dest.IngredientsDetail, opt =>
            {
                opt.MapFrom(src => src.IngredientDetails);
            });
            CreateMap<IngredientDetailForDetailDto, IngredientDetail>();


            CreateMap<IngredientHeader, IngredientForListDto>()
            .ForMember(dest => dest.ItemName, opt =>
            {
                opt.MapFrom(src => src.Item.Name);
            });

            CreateMap<EmployeeForDetailDto, Employee>();
            CreateMap<Employee, EmployeeForDetailDto>();
            CreateMap<Employee, EmployeeForListDto>()
            .ForMember(dest => dest.TypeName, opt =>
            {//0-permanent,1-daily,2-contract
                opt.MapFrom(src => src.Type == 0 ? "Permanent" : src.Type == 1 ? "Daily" : src.Type == 2 ? "Contract" : "Other");
            })
            .ForMember(dest => dest.RoleName, opt =>
            {//0-Manager,1-Cashier,2-Baker,3-counter,4-waiter
                opt.MapFrom(src => src.Role == 0 ? "Manager" : src.Role == 1 ? "Cashier" : src.Role == 2 ? "Baker" : src.Role == 3 ? "Counter" : src.Role == 4 ? "Waiter" : "Random");
            });

            CreateMap<Routine, RoutineForDetailDto>()
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime.Value.ToString()))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime.Value.ToString()))
            .ForMember(src => src.Employee, opt => opt.MapFrom(src => src.Employee))
            .ForMember(src => src.Date, opt => opt.MapFrom(src => src.Date.DashedDate()))
            .ForMember(src => src.RoleId, opt => opt.MapFrom(src => src.Employee.Role));

            CreateMap<RoutineForDetailDto, Routine>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(dest => DateTime.Parse(dest.Date)))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(dest => TimeSpan.Parse(dest.StartTime)))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(dest => TimeSpan.Parse(dest.EndTime)));
        }
    }
}