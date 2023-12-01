import {HttpClient, HttpParams} from "@angular/common/http";
import {Injectable} from "@angular/core";

@Injectable()
export class ApiService {
  // tokenType:string | null = "";
  // accessToken:string | null = "";

  constructor(public httpClient: HttpClient) {

  }

  async getAsync(url: string, params: HttpParams): Promise<any> {
    return this.httpClient.get(url, {
      params: params
    }).toPromise();
  }

  async postAsync(url: string, body: any, params: HttpParams): Promise<any> {
    return this.httpClient.post(url, body, {
      params: params
    }).toPromise();
  }

  async putAsync(url: string, body: any, params: HttpParams): Promise<any> {
    return this.httpClient.put(url, body, {
      params: params
    }).toPromise();
  }

  async deleteAsync(url: string, params: HttpParams): Promise<any> {
    return this.httpClient.delete(url, {
      params: params
    }).toPromise();
  }

  // // getToken() {
  // //     this.tokenType = localStorage.getItem('token_type');
  // //     this.accessToken = localStorage.getItem('access_token');
  // //     if (!this.accessToken || !this.tokenType) {
  // //         window.location.href = `${environment.authenHost}/login?redirectUrl=${environment.webHost}`
  // //         return tr
  // //     }
  // //     return
  // // }
  //
  // get(url: string, params: HttpParams): Observable<any> {
  //     //let headers = this.buildHeader();
  //
  //     return this.httpClient.get<any>(url,
  //         {
  //             params: params,
  //         })
  //         //.pipe(
  //            // catchError(this.errorHandler));
  // }
  //
  // post(url: string, body: any, params: HttpParams) {
  //     //let headers = this.buildHeader();
  //
  //     return this.httpClient.post<any>(url, body, {
  //         params: params,
  //     })
  //         // .pipe(
  //         //     catchError(this.errorHandler));
  // }
  //
  // put(url: string, body: any, params: HttpParams) {
  //     //let headers = this.buildHeader();
  //
  //     return this.httpClient.put<any>(url, body, {
  //         params: params,
  //     })
  //         //.pipe(
  //          //   catchError(this.errorHandler));
  // }
  //
  // // private buildHeader() {
  // //     if (!this.getToken()) {
  // //         return;
  // //     }
  //
  // //     let headers = new HttpHeaders();
  // //     headers = headers.append("Authorization", `${this.tokenType} ${this.accessToken}`);
  //
  // //     return headers;
  // // }
  //
  // // errorHandler(error: HttpErrorResponse) {
  // //     if (error.status == 401 || error.status == 403) {
  // //         window.location.href = `${environment.authenHost}/login?redirectUrl=${environment.webHost}`;
  // //     }
  // //     return of(0);
  // // }
}
