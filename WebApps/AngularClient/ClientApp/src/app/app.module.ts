import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { CatalogComponent } from './catalog/catalog.component';
import { ProductListComponent } from './catalog/product-list/product-list.component';
import { ProductComponent } from './catalog/product/product.component';
import { AppRoutingModule } from './app-routing.module';
import { SearchMenuComponent } from './search-menu/search-menu.component';
import { CatalogService } from './core/service/catalog.service';
import { ApiService } from './core/service/api.service';
import {BasicGridComponent} from "./core/shared/grids/basic-grid/basic-grid.component";
import {NgbAlertModule, NgbModule, NgbPaginationModule} from '@ng-bootstrap/ng-bootstrap';
import { CatalogAdminComponent } from './catalog-admin/catalog-admin.component';
import { CategoryAdminComponent } from './catalog-admin/category-admin/category-admin.component';
import { ProductAdminComponent } from './catalog-admin/product-admin/product-admin.component';
import { RouterModule } from '@angular/router';
import { CategoryViewModalComponent } from './core/shared/modals/category-modals/category-view-modal/category-view-modal.component';
import { CategoryCreateModalComponent } from './core/shared/modals/category-modals/category-create-modal/category-create-modal.component';
import { CategoryUpdateModalComponent } from './core/shared/modals/category-modals/category-update-modal/category-update-modal.component';
import { CategoryDeleteModalComponent } from './core/shared/modals/category-modals/category-delete-modal/category-delete-modal.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    CatalogComponent,
    ProductListComponent,
    ProductComponent,
    SearchMenuComponent,
    CatalogAdminComponent,
    ProductAdminComponent,
    CategoryAdminComponent,
    CategoryCreateModalComponent,
    CategoryViewModalComponent,
    CategoryUpdateModalComponent,
    CategoryDeleteModalComponent,

    BasicGridComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    NgbModule,
    NgbPaginationModule,
    NgbAlertModule,
    RouterModule,
  ],
  providers: [
    CatalogService,
    ApiService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
