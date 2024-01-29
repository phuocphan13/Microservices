import {NgModule} from "@angular/core";
import {ApiService} from "./api.service";
import {CatalogService} from "./catalog/catalog.service";

@NgModule({
  declarations: [],
  providers: [
    ApiService,
    CatalogService
  ]
})
export class ServiceModule {
}
