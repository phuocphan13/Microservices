import {HttpParams} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {environment} from "src/environments/environment";
import {ApiService} from "./api.service";
import { ProductDetail } from "../models/product-detail.model";
import { ProductSummary } from "../models/product-summary.model";

@Injectable()
export class CatalogService {
  apiName: string = "Catalog";

  constructor(public apiService: ApiService) {

  }

  async getProductsAsync(): Promise<ProductSummary[]> {
    return await this.apiService.getAsync(`${environment.baseApiUrl}/${this.apiName}/GetProducts`, new HttpParams());
  }

  async getProductByIdAsync(id: string): Promise<ProductDetail> {
    return await this.apiService.getAsync(`${environment.baseApiUrl}/${this.apiName}/GetProductById/${id}`, new HttpParams());
  }

  async getProductByCategoryAsync(category: string): Promise<ProductSummary> {
    return this.apiService.getAsync(`${environment.baseApiUrl}/${this.apiName}/GetProductByCategory/${category}`, new HttpParams());
  }

  async createProductAsync(catalogDetail: any): Promise<ProductDetail> {
    return this.apiService.postAsync(`${environment.baseApiUrl}/${this.apiName}/CreateProduct`, catalogDetail, new HttpParams());
  }

  async updateProductAsync(catalogDetail: any): Promise<ProductDetail> {
    return this.apiService.postAsync(`${environment.baseApiUrl}/${this.apiName}/UpdateProduct`, catalogDetail, new HttpParams());
  }

  async deleteProductAsync(id: string): Promise<boolean> {
    return await this.apiService.postAsync(`${environment.baseApiUrl}/${this.apiName}/DeleteProduct`, id, new HttpParams());
  }
}
