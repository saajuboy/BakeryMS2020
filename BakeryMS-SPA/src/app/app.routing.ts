import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Import Containers
import { DefaultLayoutComponent } from './containers';

import { P404Component } from './views/error/404.component';
import { P500Component } from './views/error/500.component';
import { LoginComponent } from './views/login/login.component';
import { RegisterComponent } from './views/user/register/register.component';
import { TestComponent } from './views/Test/Test.component';
import { UserListComponent } from './views/User/user-list/user-list.component';
import { PurchaseOrderListComponent } from './views/Inventory/PurchaseOrderList/PurchaseOrderList.component';

import { AuthGuard } from './_guards/auth.guard';
import { AdminGuard } from './_guards/admin.guard';
import { RoleGuard } from './_guards/role.guard';
import { PurchaseOrderCreateComponent } from './views/Inventory/PurchaseOrderCreate/PurchaseOrderCreate.component';
import { ItemListComponent } from './views/Inventory/Item/ItemList/ItemList.component';
import { ItemCreateComponent } from './views/Inventory/Item/ItemCreate/ItemCreate.component';
import { ItemCategoryComponent } from './views/MasterData/itemCategory/itemCategory.component';
import { SupplierComponent } from './views/MasterData/supplier/supplier.component';
import { UnitComponent } from './views/MasterData/unit/unit.component';
import { ProductionOrderListComponent } from './views/Manufacturing/ProductionOrderList/ProductionOrderList.component';
import { ProductionOrderCreateComponent } from './views/Manufacturing/ProductionOrderCreate/ProductionOrderCreate.component';
import { IngredientListComponent } from './views/Manufacturing/IngredientList/IngredientList.component';
import { IngredientCreateComponent } from './views/Manufacturing/IngredientCreate/IngredientCreate.component';
import { EmployeeComponent } from './views/HumanResource/Employee/Employee.component';
import { RoutineComponent } from './views/HumanResource/Routine/Routine.component';
import { ProductionPlanListComponent } from './views/Manufacturing/ProductionPlanList/ProductionPlanList.component';
import { ProductionPlanCreateComponent } from './views/Manufacturing/ProductionPlanCreate/ProductionPlanCreate.component';
import { ItemAcceptanceComponent } from './views/Inventory/ItemAcceptance/ItemAcceptance.component';
import { GRNComponent } from './views/Inventory/GRN/GRN.component';
import { AvailableItemsComponent } from './views/Inventory/AvailableItems/AvailableItems.component';
import { SalesCreateComponent } from './views/POS/SalesCreate/SalesCreate.component';
import { SalesListComponent } from './views/POS/SalesList/SalesList.component';
import { TransactionsComponent } from './views/POS/Transactions/Transactions.component';
import { ConfigurationComponent } from './views/Configuration/Configuration.component';
import { CustomerComponent } from './views/MasterData/customer/customer.component';
import { NotificationComponent } from './views/Notification/Notification.component';
import { MasterReportsComponent } from './views/Reports/masterReports/masterReports.component';
import { SalesReportComponent } from './views/Reports/SalesReport/SalesReport.component';
import { InventoryReportsComponent } from './views/Reports/inventoryReports/inventoryReports.component';
import { ManufacturingReportsComponent } from './views/Reports/manufacturingReports/manufacturingReports.component';
import { ControlProcedureComponent } from './views/ControlProcedure/ControlProcedure.component';


export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full', },
  { path: '404', component: P404Component, data: { title: 'Page 404' } },
  { path: '500', component: P500Component, data: { title: 'Page 500' } },
  { path: 'login', component: LoginComponent, data: { title: 'Login Page' } },

  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    component: DefaultLayoutComponent,
    data: { title: 'Home' },
    children: [
      { path: 'test', component: TestComponent, data: { title: 'Test Module' } },

      { path: 'base', loadChildren: () => import('./views/base/base.module').then(m => m.BaseModule) },
      { path: 'buttons', loadChildren: () => import('./views/buttons/buttons.module').then(m => m.ButtonsModule) },
      { path: 'charts', loadChildren: () => import('./views/chartjs/chartjs.module').then(m => m.ChartJSModule) },
      { path: 'dashboard', loadChildren: () => import('./views/dashboard/dashboard.module').then(m => m.DashboardModule) },
      { path: 'icons', loadChildren: () => import('./views/icons/icons.module').then(m => m.IconsModule) },
      { path: 'notifications', loadChildren: () => import('./views/notifications/notifications.module').then(m => m.NotificationsModule) },
      { path: 'theme', loadChildren: () => import('./views/theme/theme.module').then(m => m.ThemeModule) },
      { path: 'widgets', loadChildren: () => import('./views/widgets/widgets.module').then(m => m.WidgetsModule) },
      {
        path: 'pos/sales/create',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: SalesCreateComponent,
        data: {
          title: 'Point Of Sale',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'pos/sales',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: SalesListComponent,
        data: {
          title: 'List Of Sales',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'pos/transactions',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: TransactionsComponent,
        data: {
          title: 'Transactions',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'inventory/item',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: AvailableItemsComponent,
        data: {
          title: 'available Item List',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'inventory/purchaseOrder',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: PurchaseOrderListComponent,
        data: {
          title: 'Purchase Order List',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager']
        }
      },
      {
        path: 'inventory/purchaseOrder/create',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: PurchaseOrderCreateComponent,
        data: {
          title: 'Create Pur.Order',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager']
        }
      },
      {
        path: 'inventory/purchaseOrder/edit/:id',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: PurchaseOrderCreateComponent,
        data: {
          title: 'Edit Pur.Order',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager']
        }
      },
      {
        path: 'inventory/purchaseOrder/reOrder/:placeId/:type',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: PurchaseOrderCreateComponent,
        data: {
          title: 'Edit Pur.Order',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager']
        }
      },
      {
        path: 'inventory/itemAcceptance',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ItemAcceptanceComponent,
        data: {
          title: 'Prod Item acceptance',
          allowedRoles: ['Admin', 'BakeryManager']
        }
      },
      {
        path: 'inventory/grn',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: GRNComponent,
        data: {
          title: 'Goods Rec.Note (GRN)',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager']
        }
      },
      {
        path: 'manufacturing/productionOrder',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ProductionOrderListComponent,
        data: {
          title: 'Production Order List',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'manufacturing/productionOrder/create',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ProductionOrderCreateComponent,
        data: {
          title: 'Create Prod.Order',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'manufacturing/productionOrder/edit/:id',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ProductionOrderCreateComponent,
        data: {
          title: 'Edit Prod.Order',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'manufacturing/productionOrder/reOrder/:placeId',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ProductionOrderCreateComponent,
        data: {
          title: 'Prod.ReOrder',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'manufacturing/productionPlan',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ProductionPlanListComponent,
        data: {
          title: 'Production Plan List',
          allowedRoles: ['Admin', 'BakeryManager']
        }
      },
      {
        path: 'manufacturing/productionPlan/create',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ProductionPlanCreateComponent,
        data: {
          title: 'Create Prod.Plan',
          allowedRoles: ['Admin', 'BakeryManager']
        }
      },
      {
        path: 'manufacturing/productionPlan/edit/:id',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ProductionPlanCreateComponent,
        data: {
          title: 'Edit Prod.Plan',
          allowedRoles: ['Admin', 'BakeryManager']
        }
      },
      {
        path: 'manufacturing/ingredient',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: IngredientListComponent,
        data: {
          title: 'Ingredient/Recipe List',
          allowedRoles: ['Admin', 'BakeryManager']
        }
      },
      {
        path: 'manufacturing/ingredient/create',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: IngredientCreateComponent,
        data: {
          title: 'Create Ingredient/Recipe',
          allowedRoles: ['Admin', 'BakeryManager']
        }
      },
      {
        path: 'manufacturing/ingredient/edit/:id',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: IngredientCreateComponent,
        data: {
          title: 'Edit Ingredient/Recipe',
          allowedRoles: ['Admin', 'BakeryManager']
        }
      },
      {
        path: 'hr/employees',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: EmployeeComponent,
        data: {
          title: 'Employee',
          allowedRoles: ['Admin', 'BakeryManager', 'OutletManager']
        }
      },
      {
        path: 'hr/routines',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: RoutineComponent,
        data: {
          title: 'Routine',
          allowedRoles: ['Admin', 'BakeryManager', 'OutletManager']
        }
      },
      {
        path: 'quality/controlProcedure',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ControlProcedureComponent,
        data: {
          title: 'Control Procedure',
          allowedRoles: ['Admin', 'BakeryManager', 'OutletManager']
        }
      },
      {
        path: 'user/register',
        runGuardsAndResolvers: 'always',
        canActivate: [AdminGuard],
        component: RegisterComponent,
        data: {
          title: 'Register'
        }
      },
      {
        path: 'user/edit/:id',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        component: RegisterComponent,
        data: {
          title: 'Edit User'
        }
      },
      {
        path: 'user/list',
        runGuardsAndResolvers: 'always',
        canActivate: [AdminGuard],
        component: UserListComponent,
        data: {
          title: 'List'
        }
      },
      {
        path: 'master/item',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ItemListComponent,
        data: {
          title: 'Item List',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'master/item/create',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ItemCreateComponent,
        data: {
          title: 'Item Create',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'master/item/edit/:id',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ItemCreateComponent,
        data: {
          title: 'Item Edit',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'master/itemCategory',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ItemCategoryComponent,
        data: {
          title: 'Item Categories',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'master/customer',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: CustomerComponent,
        data: {
          title: 'Customers',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'master/supplier',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: SupplierComponent,
        data: {
          title: 'Suppliers',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'master/unit',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: UnitComponent,
        data: {
          title: 'Units',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'configuration',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ConfigurationComponent,
        data: {
          title: 'Configuration',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'notification',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        component: NotificationComponent,
        data: {
          title: 'Notifications',
        }
      },
      {
        path: 'reports/master',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        component: MasterReportsComponent,
        data: {
          title: 'Master reports',
        }
      },
      {
        path: 'reports/sales',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        component: SalesReportComponent,
        data: {
          title: 'Sales reports',
        }
      },
      {
        path: 'reports/inventory',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        component: InventoryReportsComponent,
        data: {
          title: 'Inventory reports',
        }
      },
      {
        path: 'reports/manufacuring',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        component: ManufacturingReportsComponent,
        data: {
          title: 'Manufacturing reports',
        }
      }
    ]
  },
  { path: '**', component: P404Component }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
