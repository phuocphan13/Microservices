import { Injectable } from "@angular/core";
import { ApiService } from "../api.service";
import { CategorySummary } from "../../models/catalog/category-models/category-summary.model";
import { environment } from "src/environments/environment";
import { HttpParams } from "@angular/common/http";
import { CategoryDetail } from "../../models/catalog/category-models/category-detail.model";
import { LoginRequest } from "../../models/identity/login-request.model";
import { LoginResponse } from "../../models/identity/login-response.model";

@Injectable()
export class IdentityService {
  apiName: string = "Identity";

  constructor(public apiService: ApiService) {

  }

  async login(request: LoginRequest): Promise<LoginResponse> {
    return await this.apiService.postAsync(`${environment.baseApiUrl}/${this.apiName}/Login`, request, new HttpParams());
  }
}
