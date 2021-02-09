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
        path: 'inventory/item',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ItemListComponent,
        data: {
          title: 'Item List',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      }, 
      {
        path: 'inventory/item/create',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ItemCreateComponent,
        data: {
          title: 'Item Create',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager', 'Cashier']
        }
      },
      {
        path: 'inventory/item/edit:id',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: ItemCreateComponent,
        data: {
          title: 'Item Edit',
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
        path: 'inventory/purchaseOrder/edit:id',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        component: PurchaseOrderCreateComponent,
        data: {
          title: 'Edit Pur.Order',
          allowedRoles: ['Admin', 'OutletManager', 'BakeryManager']
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
        canActivate: [AdminGuard],
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
