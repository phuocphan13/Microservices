import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

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

    BasicGridComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    NgbModule,
    NgbPaginationModule,
    NgbAlertModule
  ],
  providers: [
    CatalogService,
    ApiService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
