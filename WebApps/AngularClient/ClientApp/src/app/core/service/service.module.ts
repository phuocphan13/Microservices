import {NgModule} from "@angular/core";
import {ApiService} from "./api.service";
import {CatalogService} from "./catalog/catalog.service";
import { IdentityService } from "./identity/identity.service";

@NgModule({
  declarations: [],
  providers: [
    ApiService,
    CatalogService,
    IdentityService
  ]
})
export class ServiceModule {
}
