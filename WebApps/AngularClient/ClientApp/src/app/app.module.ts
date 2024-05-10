import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { AppRoutingModule } from './app-routing.module';
import { CatalogService } from './core/service/catalog/catalog.service';
import { ApiService } from './core/service/api.service';
import { BasicGridComponent } from "./core/shared/grids/basic-grid/basic-grid.component";
import { NgbAlertModule, NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { CategoryService } from './core/service/catalog/category.service';
import { CatalogAdminComponent } from './pages/admin-pages/catalog-admin/catalog-admin.component';
import { NavMenuComponent } from "./core/components/nav-menu/nav-menu.component";
import { CatalogComponent } from "./pages/client-pages/catalog/catalog.component";
import { ProductListComponent } from "./pages/client-pages/catalog/product-list/product-list.component";
import { ProductComponent } from "./pages/client-pages/catalog/product/product.component";
import { SearchMenuComponent } from "./core/shared/items/search-menu/search-menu.component";
import { ProductAdminComponent } from "./pages/admin-pages/catalog-admin/product-admin/product-admin.component";
import { CategoryAdminComponent } from "./pages/admin-pages/catalog-admin/category-admin/category-admin.component";
import { CategoryModalComponent } from "./core/shared/modals/catalog-modals/category-modal/category-modal.component";
import { ConfirmationModalComponent } from "./core/shared/modals/common/confirmation-modal/confirmation-modal.component";
import { UnsavedConfirmModalComponent } from "./core/shared/modals/common/unsaved-confirm-modal/unsaved-confirm-modal.component";
import { LogInComponent } from "./pages/identity-pages/log-in/log-in.component";
import { ServiceModule } from "./core/service/service.module";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CatalogComponent,
    ProductListComponent,
    ProductComponent,
    SearchMenuComponent,
    CatalogAdminComponent,
    ProductAdminComponent,
    CategoryAdminComponent,
    CategoryModalComponent,
    BasicGridComponent,
    ConfirmationModalComponent,
    UnsavedConfirmModalComponent,
    LogInComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    NgbModule,
    NgbPaginationModule,
    NgbAlertModule,
    RouterModule,
    ReactiveFormsModule,
    ServiceModule
  ],
  providers: [
    CatalogService,
    CategoryService,
    ApiService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
