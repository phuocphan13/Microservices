import { Injectable } from "@angular/core";
import { ApiService } from "../api.service";
import { SubCategorySummary } from "../../models/catalog/subcategory-models/subcategory-summary.model";
import { environment } from "src/environments/environment";
import { HttpParams } from "@angular/common/http";
import { SubCategoryDetail } from "../../models/catalog/subcategory-models/subcategory-detail.model";


@Injectable()
export class SubCategoryService {
    apiName: string = "Subcategory";

    constructor(public apiService: ApiService) {
    }

    //GET Sub-Categories
    async getSubCategoryAsync(): Promise<SubCategorySummary[]> {
        return await this.apiService.getAsync(`${environment.baseApiUrl}/${this.apiName}/GetSubCategories`, new HttpParams());
    }

    //CREAT Sub-Category
    async creatSubCategoryAsync (subCategoryDetail: any): Promise<SubCategoryDetail> {
        return await this.apiService.postAsync(`${environment.baseApiUrl}/${this.apiName}/CreateSubCategory`,subCategoryDetail, new HttpParams());
    }

    //UPDATE Sub-Category
    async updateSubCategoryAsync (subCategoryDetail: any): Promise<SubCategoryDetail> {
        return await this.apiService.postAsync(`${environment.baseApiUrl}/${this.apiName}/UpdateSubCategory`,subCategoryDetail, new HttpParams());
    }

    //DELETE Sub-Category
    async deleteSubCategoryAsync(id: string): Promise<boolean> {
        return await this.apiService.deleteAsync(`${environment.baseApiUrl}/${this.apiName}/DeleteSubCategory/${id}`, new HttpParams());
      }
}