import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};

import { AppComponent } from './app.component';

// Import containers
import { DefaultLayoutComponent } from './containers';

import { P404Component } from './views/error/404.component';
import { P500Component } from './views/error/500.component';


const APP_CONTAINERS = [
  DefaultLayoutComponent
];

import {
  AppAsideModule,
  AppBreadcrumbModule,
  AppHeaderModule,
  AppFooterModule,
  AppSidebarModule,
} from '@coreui/angular';

// Import routing module
import { AppRoutingModule } from './app.routing';

import { JwtModule } from '@auth0/angular-jwt';
import { HttpClientModule } from '@angular/common/http';

// Import 3rd party components
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ChartsModule } from 'ng2-charts';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgWizardConfig, NgWizardModule, THEME } from 'ng-wizard';
import { JwPaginationModule } from 'jw-angular-pagination';

// Import services, guards
import { AuthService } from './_services/auth.service';
import { AlertifyService } from './_services/alertify.service';
import { AuthGuard } from './_guards/auth.guard';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { environment } from '../environments/environment';
import { AdminGuard } from './_guards/admin.guard';
import { RoleGuard } from './_guards/role.guard';
import { UtilityService } from './_services/utility.service';
import { MasterService } from './_services/master.service';
import { InventoryService } from './_services/inventory.service';
import { ManufacturingService } from './_services/manufacturing.service';
import { HumanResourceService } from './_services/humanResource.service';


// import Components
import { UserListComponent } from './views/User/user-list/user-list.component';
import { TestComponent } from './views/Test/Test.component';
import { LoginComponent } from './views/login/login.component';
import { RegisterComponent } from './views/user/register/register.component';
import { PurchaseOrderCreateComponent } from './views/Inventory/PurchaseOrderCreate/PurchaseOrderCreate.component';
import { PurchaseOrderListComponent } from './views/Inventory/PurchaseOrderList/PurchaseOrderList.component';
import { ItemCreateComponent } from './views/Inventory/Item/ItemCreate/ItemCreate.component';
import { ItemListComponent } from './views/Inventory/Item/ItemList/ItemList.component';
import { ItemCategoryComponent } from './views/MasterData/itemCategory/itemCategory.component';
import { UnitComponent } from './views/MasterData/unit/unit.component';
import { SupplierComponent } from './views/MasterData/supplier/supplier.component';
import { ProductionOrderCreateComponent } from './views/Manufacturing/ProductionOrderCreate/ProductionOrderCreate.component';
import { ProductionOrderListComponent } from './views/Manufacturing/ProductionOrderList/ProductionOrderList.component';
import { IngredientCreateComponent } from './views/Manufacturing/IngredientCreate/IngredientCreate.component';
import { IngredientListComponent } from './views/Manufacturing/IngredientList/IngredientList.component';
import { EmployeeComponent } from './views/HumanResource/Employee/Employee.component';
import { RoutineComponent } from './views/HumanResource/Routine/Routine.component';
import { ProductionPlanCreateComponent } from './views/Manufacturing/ProductionPlanCreate/ProductionPlanCreate.component';
import { ProductionPlanListComponent } from './views/Manufacturing/ProductionPlanList/ProductionPlanList.component';
import { ItemAcceptanceComponent } from './views/Inventory/ItemAcceptance/ItemAcceptance.component';
import { GRNComponent } from './views/Inventory/GRN/GRN.component';
import { AvailableItemsComponent } from './views/Inventory/AvailableItems/AvailableItems.component';


// token getter function to automatically intercept http requests
export function tokenGetter() {
  // console.log(localStorage.getItem('token'));
  return localStorage.getItem('token');
}

const ngWizardConfig: NgWizardConfig = {
  theme: THEME.circles
};

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AppAsideModule,
    AppBreadcrumbModule.forRoot(),
    AppFooterModule,
    AppHeaderModule,
    AppSidebarModule,
    PerfectScrollbarModule,
    BsDropdownModule.forRoot(),
    ModalModule.forRoot(),
    TabsModule.forRoot(),
    Ng2SearchPipeModule,
    NgWizardModule.forRoot(ngWizardConfig),
    JwPaginationModule,
    ChartsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: [environment.whiteListedDomains],
        blacklistedRoutes: [environment.blackListedDomains]
      },
    })
  ],
  declarations: [
    AppComponent,
    ...APP_CONTAINERS,
    P404Component,
    P500Component,
    LoginComponent,
    RegisterComponent,
    TestComponent,
    UserListComponent,
    PurchaseOrderCreateComponent,
    PurchaseOrderListComponent,
    ItemCreateComponent,
    ItemListComponent,
    ItemCategoryComponent,
    UnitComponent,
    SupplierComponent,
    ProductionOrderCreateComponent,
    ProductionOrderListComponent,
    IngredientCreateComponent,
    IngredientListComponent,
    EmployeeComponent,
    RoutineComponent,
    ProductionPlanCreateComponent,
    ProductionPlanListComponent,
    ItemAcceptanceComponent,
    GRNComponent,
    AvailableItemsComponent
  ],
  providers: [{
    provide: LocationStrategy,
    useClass: HashLocationStrategy
  },
    AuthService,
    AlertifyService,
    UtilityService,
    MasterService,
    InventoryService,
    ManufacturingService,
    HumanResourceService,
    AuthGuard,
    AdminGuard,
    RoleGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
