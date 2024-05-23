import { Injectable } from "@angular/core";
import { ApiService } from "../api.service";
import { CategorySummary } from "../../models/catalog/category-models/category-summary.model";
import { environment } from "src/environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { CategoryDetail } from "../../models/catalog/category-models/category-detail.model";
import { lastValueFrom } from "rxjs";

@Injectable()
export class CategoryService {
  apiName: string = "Category";

  constructor(public apiService: ApiService, private httpClient: HttpClient) {

  }

  //Todo: Example for HttpClient
  async getCategoriesAsync(): Promise<CategorySummary[]> {
    let observableResult = this.httpClient.get(`${environment.baseApiUrl}/${this.apiName}/GetCategories`);

    return await lastValueFrom(observableResult) as CategorySummary[];
  }

  async getCategoryByIdAsync(id: string): Promise<CategoryDetail> {
    return await this.apiService.getAsync(`${environment.baseApiUrl}/${this.apiName}/GetCategoryById/${id}`, new HttpParams());
  }

  async getCategoryByNameAsync(name: string): Promise<CategoryDetail> {
    return await this.apiService.getAsync(`${environment.baseApiUrl}/${this.apiName}/GetCategoryByName/${name}`, new HttpParams());
  }

  async createCategoryAsync(categoryDetail: any): Promise<CategoryDetail> {
    return await this.apiService.postAsync(`${environment.baseApiUrl}/${this.apiName}/CreateCategory`, categoryDetail, new HttpParams());
  }

  async updateCategoryAsync(categoryDetail: any): Promise<CategoryDetail> {
    return await this.apiService.putAsync(`${environment.baseApiUrl}/${this.apiName}/UpdateCategory`, categoryDetail, new HttpParams());
  }

  async deleteCategoryAsync(id: string): Promise<boolean> {
    return await this.apiService.deleteAsync(`${environment.baseApiUrl}/${this.apiName}/DeleteCategory/${id}`, new HttpParams());
  }
}
