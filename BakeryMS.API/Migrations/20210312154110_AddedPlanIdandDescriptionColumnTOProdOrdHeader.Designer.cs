﻿// <auto-generated />
using System;
using BakeryMS.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BakeryMS.API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210312154110_AddedPlanIdandDescriptionColumnTOProdOrdHeader")]
    partial class AddedPlanIdandDescriptionColumnTOProdOrdHeader
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BakeryMS.API.Models.BusinessPlace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EstablishedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RegistrationEndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RegistrationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RegistrationStartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("BusinessPlaces");
                });

            modelBuilder.Entity("BakeryMS.API.Models.HumanResource.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmployeeNumber")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNotActive")
                        .HasColumnType("bit");

                    b.Property<string>("NIC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("BakeryMS.API.Models.HumanResource.Routine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BusinessPlaceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<TimeSpan?>("EndTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan?>("StartTime")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("BusinessPlaceId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Routines");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Inventory.PurchaseOrderDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int?>("ItemId")
                        .HasColumnType("int");

                    b.Property<decimal>("LineTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("OrderQty")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("POHeaderId")
                        .HasColumnType("int");

                    b.Property<decimal?>("ReceivedQuantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("RejectedQuantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("StockedQuantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("POHeaderId");

                    b.ToTable("PurchaseOrderDetails");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Inventory.PurchaseOrderHeader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeliveryMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PONumber")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int?>("SupplierId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("isFromOutlet")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("SupplierId");

                    b.ToTable("PurchaseOrderHeaders");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Inventory.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int?>("ItemCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int?>("UnitId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ItemCategoryId");

                    b.HasIndex("UnitId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("BakeryMS.API.Models.ItemCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ItemCategories");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Machinery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BusinessPlaceId")
                        .HasColumnType("int");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PurchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessPlaceId");

                    b.ToTable("Machineries");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.IngredientDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("IngredientsHeaderId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("IngredientsHeaderId");

                    b.HasIndex("ItemId");

                    b.ToTable("IngredientDetails");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.IngredientHeader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<string>("Method")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ServingSize")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("IngredientHeaders");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.ProductionOrderDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductionOrderHeaderId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("ProductionOrderHeaderId");

                    b.ToTable("ProductionOrderDetails");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.ProductionOrderHeader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BusinessPlaceId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EnteredDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNotEditable")
                        .HasColumnType("bit");

                    b.Property<int?>("PlanId")
                        .HasColumnType("int");

                    b.Property<int>("ProductionOrderNo")
                        .HasColumnType("int");

                    b.Property<DateTime?>("RequiredDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SessionId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BusinessPlaceId");

                    b.HasIndex("SessionId");

                    b.HasIndex("UserId");

                    b.ToTable("ProductionOrderHeaders");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.ProductionPlanDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductionPlanHeaderId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("ProductionPlanHeaderId");

                    b.ToTable("ProductionPlanDetails");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.ProductionPlanHeader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BusinessPlaceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNotEditable")
                        .HasColumnType("bit");

                    b.Property<int>("ProductionSessionId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BusinessPlaceId");

                    b.HasIndex("ProductionSessionId");

                    b.HasIndex("UserId");

                    b.ToTable("ProductionPlanHeaders");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.ProductionPlanMachine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MachineryId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductionPlanHeaderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MachineryId");

                    b.HasIndex("ProductionPlanHeaderId");

                    b.ToTable("ProductionPlanMachines");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.ProductionPlanRecipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("productionPlanHeaderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("productionPlanHeaderId");

                    b.ToTable("ProductionPlanRecipes");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.ProductionPlanWorker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductionPlanHeaderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ProductionPlanHeaderId");

                    b.ToTable("ProductionPlanWorkers");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.ProductionSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<TimeSpan?>("EndTime")
                        .HasColumnType("time");

                    b.Property<string>("Session")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan?>("StartTime")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.ToTable("ProductionSessions");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Profile.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Profile.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ContactNumber")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastActive")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhotoPublicId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Profile.UserRolesMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("RolesId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RolesId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRolesMappings");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("BakeryMS.API.Models.HumanResource.Routine", b =>
                {
                    b.HasOne("BakeryMS.API.Models.BusinessPlace", "BusinessPlace")
                        .WithMany()
                        .HasForeignKey("BusinessPlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BakeryMS.API.Models.HumanResource.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BakeryMS.API.Models.Inventory.PurchaseOrderDetail", b =>
                {
                    b.HasOne("BakeryMS.API.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId");

                    b.HasOne("BakeryMS.API.Models.Inventory.PurchaseOrderHeader", "POHeader")
                        .WithMany("PurchaseOrderDetail")
                        .HasForeignKey("POHeaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BakeryMS.API.Models.Inventory.PurchaseOrderHeader", b =>
                {
                    b.HasOne("BakeryMS.API.Models.Inventory.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierId");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Item", b =>
                {
                    b.HasOne("BakeryMS.API.Models.ItemCategory", "ItemCategory")
                        .WithMany("Items")
                        .HasForeignKey("ItemCategoryId");

                    b.HasOne("BakeryMS.API.Models.Unit", "Unit")
                        .WithMany("Items")
                        .HasForeignKey("UnitId");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Machinery", b =>
                {
                    b.HasOne("BakeryMS.API.Models.BusinessPlace", "BusinessPlace")
                        .WithMany()
                        .HasForeignKey("BusinessPlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.IngredientDetail", b =>
                {
                    b.HasOne("BakeryMS.API.Models.Production.IngredientHeader", "IngredientsHeader")
                        .WithMany("IngredientsDetail")
                        .HasForeignKey("IngredientsHeaderId");

                    b.HasOne("BakeryMS.API.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.IngredientHeader", b =>
                {
                    b.HasOne("BakeryMS.API.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.ProductionOrderDetail", b =>
                {
                    b.HasOne("BakeryMS.API.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BakeryMS.API.Models.Production.ProductionOrderHeader", "ProductionOrderHeader")
                        .WithMany("ProductionOrderDetails")
                        .HasForeignKey("ProductionOrderHeaderId");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.ProductionOrderHeader", b =>
                {
                    b.HasOne("BakeryMS.API.Models.BusinessPlace", "BusinessPlace")
                        .WithMany()
                        .HasForeignKey("BusinessPlaceId");

                    b.HasOne("BakeryMS.API.Models.Production.ProductionSession", "Session")
                        .WithMany()
                        .HasForeignKey("SessionId");

                    b.HasOne("BakeryMS.API.Models.Profile.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.ProductionPlanDetail", b =>
                {
                    b.HasOne("BakeryMS.API.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BakeryMS.API.Models.Production.ProductionPlanHeader", "ProductionPlanHeader")
                        .WithMany("ProductionPlanDetails")
                        .HasForeignKey("ProductionPlanHeaderId");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.ProductionPlanHeader", b =>
                {
                    b.HasOne("BakeryMS.API.Models.BusinessPlace", "BusinessPlace")
                        .WithMany()
                        .HasForeignKey("BusinessPlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BakeryMS.API.Models.Production.ProductionSession", "ProductionSession")
                        .WithMany()
                        .HasForeignKey("ProductionSessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BakeryMS.API.Models.Profile.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.ProductionPlanMachine", b =>
                {
                    b.HasOne("BakeryMS.API.Models.Machinery", "Machinery")
                        .WithMany()
                        .HasForeignKey("MachineryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BakeryMS.API.Models.Production.ProductionPlanHeader", "ProductionPlanHeader")
                        .WithMany("ProductionPlanMachines")
                        .HasForeignKey("ProductionPlanHeaderId");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.ProductionPlanRecipe", b =>
                {
                    b.HasOne("BakeryMS.API.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BakeryMS.API.Models.Production.ProductionPlanHeader", "productionPlanHeader")
                        .WithMany("ProductionPlanRecipes")
                        .HasForeignKey("productionPlanHeaderId");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Production.ProductionPlanWorker", b =>
                {
                    b.HasOne("BakeryMS.API.Models.HumanResource.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BakeryMS.API.Models.Production.ProductionPlanHeader", "ProductionPlanHeader")
                        .WithMany("ProductionPlanWorkers")
                        .HasForeignKey("ProductionPlanHeaderId");
                });

            modelBuilder.Entity("BakeryMS.API.Models.Profile.UserRolesMapping", b =>
                {
                    b.HasOne("BakeryMS.API.Models.Profile.Roles", "Roles")
                        .WithMany()
                        .HasForeignKey("RolesId");

                    b.HasOne("BakeryMS.API.Models.Profile.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
