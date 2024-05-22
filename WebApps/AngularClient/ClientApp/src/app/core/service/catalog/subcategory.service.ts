import { Injectable } from "@angular/core";
import { ApiService } from "../api.service";
import { SubCategorySummary } from "../../models/catalog/subcategory-models/subcategory-summary.model";
import { environment } from "src/environments/environment";
import { HttpParams } from "@angular/common/http";


@Injectable()
export class SubCategoryService {
    apiName: string = "Subcategory";

    constructor(public apiService: ApiService) {
    }

    //get Sub-Categories
    async getSubCategoryAsync(): Promise<SubCategorySummary[]> {
        return await this.apiService.getAsync(`${environment.baseApiUrl}/${this.apiName}/GetSubCategories`, new HttpParams())
    }
}