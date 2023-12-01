import {NgModule} from "@angular/core";
import {ApiService} from "./api.service";
import {CatalogService} from "./catalog.service";

@NgModule({
  declarations: [],
  providers: [
    ApiService,
    CatalogService
  ]
})
export class ServiceModule {
}
