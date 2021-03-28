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
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
            .ForMember(dest => dest.BusinessPlaceName, opt => opt.MapFrom(src => src.BusinessPlace.Name))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate.ToShortDateString()))
            .ForMember(dest => dest.DeliveryDate, opt => opt.MapFrom(src => src.DeliveryDate.ToShortDateString()));

            CreateMap<PurchaseOrderHeader, POHForDetailDto>()
            .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.Supplier.Id))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate.DashedDate()))
            .ForMember(dest => dest.DeliveryDate, opt => opt.MapFrom(src => src.DeliveryDate.DashedDate()))
            .ForMember(dest => dest.PODetail, opt => opt.MapFrom(src => src.PurchaseOrderDetail));

            CreateMap<PurchaseOrderDetail, PODForDetailDto>()
            .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item.Name))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate.DashedDate()));

            CreateMap<POHForDetailDto, PurchaseOrderHeader>()
            .ForMember(dest => dest.PurchaseOrderDetail, opt => opt.MapFrom(src => src.PODetail))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.Parse(src.OrderDate)))
            .ForMember(dest => dest.DeliveryDate, opt => opt.MapFrom(src => DateTime.Parse(src.DeliveryDate)));

            CreateMap<PODForDetailDto, PurchaseOrderDetail>()
            .ForPath(dest => dest.Item.Id, opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => DateTime.Parse(src.DueDate)));

            CreateMap<GRNHeader, GRNHeaderForDetailDto>()
            .ForMember(dest => dest.ReceivedDate, opt => opt.MapFrom(src => src.ReceivedDate.DashedDate()))
            .ForMember(dest => dest.GRNDetails, opt => opt.MapFrom(src => src.GRNDetails));

            CreateMap<GRNHeaderForDetailDto, GRNHeader>()
            .ForMember(dest => dest.ReceivedDate, opt => opt.MapFrom(src => DateTime.Parse(src.ReceivedDate)))
            .ForMember(dest => dest.GRNDetails, opt => opt.MapFrom(src => src.GRNDetails));

            CreateMap<GRNDetailForDetailDto, GRNDetail>();

            CreateMap<GRNDetail, GRNDetailForDetailDto>()
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name))
            .ForMember(dest => dest.ItemCode, opt => opt.MapFrom(src => src.Item.Code));

            CreateMap<Supplier, SupplierDto>();

            CreateMap<SupplierDto, Supplier>();

            CreateMap<Item, ItemForListDto>()
            .ForMember(dest => dest.ItemCategory, opt => opt.MapFrom(src => src.ItemCategory.Description))
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit.Description));

            CreateMap<Item, ItemForDetailDto>();
            CreateMap<ItemForDetailDto, Item>();

            CreateMap<ItemCategory, ItemCategoryForDetailDto>();
            CreateMap<ItemCategoryForDetailDto, ItemCategory>();

            CreateMap<Unit, UnitForDetailDto>();
            CreateMap<UnitForDetailDto, Unit>();

            //production order
            CreateMap<ProductionOrderHeader, ProdOrderHeaderForDetailDto>()
            .ForMember(dest => dest.Session, opt => opt.MapFrom(src => src.Session.Session))
            .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src.Session.Id))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.BusinessPlace, opt => opt.MapFrom(src => src.BusinessPlace.Name))
            .ForMember(dest => dest.BusinessPlaceId, opt => opt.MapFrom(src => src.BusinessPlace.Id))
            .ForMember(dest => dest.RequiredDate, opt => opt.MapFrom(src => src.RequiredDate.Value.DashedDate()))
            .ForMember(dest => dest.EnteredDate, opt => opt.MapFrom(src => src.EnteredDate.Value.DashedDate()));

            CreateMap<ProductionOrderDetail, ProdOrderDetailForDetailDto>()
            .ForMember(src => src.Item, opt => opt.MapFrom(dest => dest.Item.Name))
            .ForMember(src => src.ItemId, opt => opt.MapFrom(dest => dest.ItemId));

            CreateMap<ProdOrderHeaderForDetailDto, ProductionOrderHeader>()
            .ForMember(dest => dest.RequiredDate, opt => opt.MapFrom(src => DateTime.Parse(src.RequiredDate)))
            .ForMember(dest => dest.EnteredDate, opt => opt.MapFrom(src => DateTime.Parse(src.EnteredDate)));

            CreateMap<ProdOrderDetailForDetailDto, ProductionOrderDetail>();

            CreateMap<ProductionOrderHeader, ProdOrderForListDto>()
            .ForMember(dest => dest.BusinessPlaceName, opt => opt.MapFrom(src => src.BusinessPlace.Name))
            .ForMember(dest => dest.SessionName, opt => opt.MapFrom(src => src.Session.Session))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.EnteredDate, opt => opt.MapFrom(src => src.EnteredDate.Value.DashedDate()))
            .ForMember(dest => dest.RequiredDate, opt => opt.MapFrom(src => src.RequiredDate.Value.DashedDate()));

            CreateMap<ProductionSession, ProdSessionForDetailDto>()
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime.Value.ToString()))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime.Value.ToString()));

            CreateMap<BusinessPlace, BusinessPlaceForDetailDto>();

            //Ingredients
            CreateMap<IngredientHeader, IngredientHeaderForDetailDto>()
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name))
            .ForMember(dest => dest.IngredientDetails, opt => opt.MapFrom(src => src.IngredientsDetail));

            CreateMap<IngredientDetail, IngredientDetailForDetailDto>()
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name));

            CreateMap<IngredientHeaderForDetailDto, IngredientHeader>()
            .ForMember(dest => dest.IngredientsDetail, opt => opt.MapFrom(src => src.IngredientDetails));

            CreateMap<IngredientDetailForDetailDto, IngredientDetail>();

            CreateMap<IngredientHeader, IngredientForListDto>()
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name));

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
            .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.DashedDate()))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Employee.Role));

            CreateMap<RoutineForDetailDto, Routine>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Parse(src.Date)))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.StartTime)))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.EndTime)));

            //Production Plan
            CreateMap<ProductionPlanHeader, ProdPlanHeaderForDetailDto>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.DashedDate()))
            .ForMember(dest => dest.ProductionPlanDetails, opt => opt.MapFrom(src => src.ProductionPlanDetails))
            .ForMember(dest => dest.ProductionPlanRecipes, opt => opt.MapFrom(src => src.ProductionPlanRecipes))
            .ForMember(dest => dest.ProductionPlanMachines, opt => opt.MapFrom(src => src.ProductionPlanMachines))
            .ForMember(dest => dest.ProductionPlanWorkers, opt => opt.MapFrom(src => src.ProductionPlanWorkers));

            CreateMap<ProductionPlanDetail, ProdPlanDetailForDetailDto>()
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name));
            CreateMap<ProductionPlanMachine, ProdPlanMachineForDetailDto>()
            .ForMember(dest => dest.MachineryName, opt => opt.MapFrom(src => src.Machinery.Name));
            CreateMap<ProductionPlanRecipe, ProdPlanRecipeForDetailDto>()
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name));
            CreateMap<ProductionPlanWorker, ProdPlanWorkerForDetailDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.Name));

            CreateMap<ProductionPlanHeader, ProdPlanForListDto>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.DashedDate()))
            .ForMember(dest => dest.BusinessPlaceName, opt => opt.MapFrom(src => src.BusinessPlace.Name))
            .ForMember(dest => dest.SessionName, opt => opt.MapFrom(src => src.ProductionSession.Session))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username));

            CreateMap<ProdPlanHeaderForDetailDto, ProductionPlanHeader>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Parse(src.Date)))
            .ForMember(dest => dest.ProductionPlanDetails, opt => opt.MapFrom(src => src.ProductionPlanDetails))
            .ForMember(dest => dest.ProductionPlanRecipes, opt => opt.MapFrom(src => src.ProductionPlanRecipes))
            .ForMember(dest => dest.ProductionPlanMachines, opt => opt.MapFrom(src => src.ProductionPlanMachines))
            .ForMember(dest => dest.ProductionPlanWorkers, opt => opt.MapFrom(src => src.ProductionPlanWorkers));

            CreateMap<ProdPlanDetailForDetailDto, ProductionPlanDetail>();
            CreateMap<ProdPlanMachineForDetailDto, ProductionPlanMachine>();
            CreateMap<ProdPlanRecipeForDetailDto, ProductionPlanRecipe>();
            CreateMap<ProdPlanWorkerForDetailDto, ProductionPlanWorker>();


            CreateMap<Machinery, MachineryDto>()
            .ForMember(dest => dest.BusinessPlaceName, opt => opt.MapFrom(src => src.BusinessPlace.Name))
            .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => src.PurchaseDate.Value.DashedDate()));

            CreateMap<MachineryDto, Machinery>()
            .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => DateTime.Parse(src.PurchaseDate)));

            CreateMap<ProductionItem, AvailableItemsDtoForList>()
            .ForMember(dest => dest.BusinessPlaceName, opt => opt.MapFrom(src => src.CurrentPlace.Name))
            .ForMember(dest => dest.ManufacturedDate, opt => opt.MapFrom(src => src.ManufacturedDate.Value.DashedDate()))
            .ForMember(dest => dest.ExpireDate, opt => opt.MapFrom(src => src.ExpireDate.Value.DashedDate()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item.Name))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Item.Code))
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Item.Unit.Description))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Item.Type));
            CreateMap<CompanyItem, AvailableItemsDtoForList>()
            .ForMember(dest => dest.BusinessPlaceName, opt => opt.MapFrom(src => src.CurrentPlace.Name))
            .ForMember(dest => dest.ManufacturedDate, opt => opt.MapFrom(src => src.ManufacturedDate.Value.DashedDate()))
            .ForMember(dest => dest.ExpireDate, opt => opt.MapFrom(src => src.ExpireDate.Value.DashedDate()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item.Name))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Item.Code))
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Item.Unit.Description))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Item.Type));
            CreateMap<RawItems, AvailableItemsDtoForList>()
            .ForMember(dest => dest.BusinessPlaceName, opt => opt.MapFrom(src => src.CurrentPlace.Name))
            .ForMember(dest => dest.ManufacturedDate, opt => opt.MapFrom(src => src.ManufacturedDate.Value.DashedDate()))
            .ForMember(dest => dest.ExpireDate, opt => opt.MapFrom(src => src.ExpireDate.Value.DashedDate()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item.Name))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Item.Code))
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Item.Unit.Description))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Item.Type));

        }
    }
}