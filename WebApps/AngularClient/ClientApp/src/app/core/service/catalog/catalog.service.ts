import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { ApiService } from "../api.service";

@Injectable()
export class CatalogService {
    constructor(public apiService: ApiService) {

    }

    GetProducts(): Observable<any> {
        let params = new HttpParams();
        return this.apiService.get(`${environment.catalogLocalHost}/GetProducts`, params)
    }

    GetCatalogByID(id:string): Observable<any> {
        let params = new HttpParams();
        params = params.append('id', id.toString());

        return this.apiService.get(`${environment.catalogLocalHost}/GetProductById`, params)
    }

    GetCatalogByCategory(): Observable<any> {
        let params = new HttpParams();

        return this.apiService.get(`${environment.catalogLocalHost}/GetProductByCategory/Smart Phone`, params)
    }

    PostCatalog(catalogDetail:any): Observable<any> {

        return this.apiService.post(`${environment.apiHost}/v1/Catalog`, catalogDetail, new HttpParams())
    }
}