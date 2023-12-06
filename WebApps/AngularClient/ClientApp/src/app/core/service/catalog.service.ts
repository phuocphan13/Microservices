import {HttpParams} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {environment} from "src/environments/environment";
import {ApiService} from "./api.service";
import { ProductSummary } from "../models/product-sumary.model";

@Injectable()
export class CatalogService {
  apiName: string = "Catalog";

  constructor(public apiService: ApiService) {

  }

  async getProductsAsync(): Promise<ProductSummary[]> {
    return await this.apiService.getAsync(`${environment.baseApiUrl}/${this.apiName}/GetProducts`, new HttpParams());
  }

  async getProductByIdAsync(id: string): Promise<any> {
    return await this.apiService.getAsync(`${environment.baseApiUrl}/${this.apiName}/GetProductById/${id}`, new HttpParams())
  }

  async getProductByCategoryAsync(category: string): Promise<any> {
    return this.apiService.getAsync(`${environment.baseApiUrl}/${this.apiName}/GetProductByCategory/${category}`, new HttpParams())
  }

  async createProductAsync(catalogDetail: any): Promise<any> {
    return this.apiService.postAsync(`${environment.baseApiUrl}/${this.apiName}/CreateProduct`, catalogDetail, new HttpParams());
  }
}
