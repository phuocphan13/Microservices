import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { lastValueFrom, of } from "rxjs";
import { CookiesService } from "./shared/cookie.service";
import { Token } from "../../common/const";
import { environment } from "../../../environments/environment";
import { AccessTokenDetail } from "../models/identity/response/access-token-detail.model";
import { Router } from "@angular/router";

@Injectable()
export class ApiService {
  // tokenType:string | null = "";
  // accessToken:string | null = "";

  constructor(public httpClient: HttpClient, private cookieService: CookiesService, private router: Router) {

  }

  async getAsync(url: string, params: HttpParams): Promise<any> {
    await this.buildHeader(params);

    let observableResult = this.httpClient.get(url, {
      params: params
    });

    return await lastValueFrom(observableResult).catch(this.errorHandler);
  }

  async postAsync(url: string, body: any, params: HttpParams): Promise<any> {
    await this.buildHeader(params);

    let observableResult = this.httpClient.post(url, body, {
      params: params
    });

    return await lastValueFrom(observableResult).catch(this.errorHandler);
  }

  async putAsync(url: string, body: any, params: HttpParams): Promise<any> {
    await this.buildHeader(params);

    let observableResult = this.httpClient.put(url, body, {
      params: params
    });

    return await lastValueFrom(observableResult).catch(this.errorHandler);
  }

  async deleteAsync(url: string, params: HttpParams): Promise<any> {
    await this.buildHeader(params);

    let observableResult = this.httpClient.delete(url, {
      params: params
    });

    return await lastValueFrom(observableResult).catch(this.errorHandler);
  }

  errorHandler(error: HttpErrorResponse) {
    if (error.status == 401 || error.status == 403 || error.status == 404 || (error.error.includes("Invalid Token"))) {
      // window.location.href = `${environment.appRoot}/log-in?applicationId=${environment.clientId}`;
      throw new Error("Error");
    }

    return of(0);
  }

  private async generateAccessTokenByRefreshTokenAsync(accountId: string, refreshToken: string) {
    let url = `${environment.baseApiUrl}/api/Identity/GenerateAccessTokenByRefreshToken`;

    let params = {
      accountId: accountId,
      refreshToken: refreshToken
    }

    let observableResult = this.httpClient.post<AccessTokenDetail>(url, {
      params: params
    });

    let result = await lastValueFrom<AccessTokenDetail>(observableResult);

    if (result) {
      this.cookieService.setCookie(Token.AccessToken, result.token!, result.expires!);
      this.cookieService.setCookie(Token.TokenType, "Bearer", result.expires!);
    }

    return `Bearer ${result.token}`;
  }

  private async buildHeader(params: HttpParams) {
    let token = await this.getToken();

    if (token) {
      params.append('Authorization', token);
    }
  }

  private async getToken() {
    let accessToken = this.cookieService.getCookie(Token.AccessToken);

    if (accessToken) {
      let tokenType = this.cookieService.getCookie(Token.TokenType);
      return `${tokenType} ${accessToken}`;
    } else {
      let refreshToken = this.cookieService.getCookie(Token.RefreshToken) as string;

      if (refreshToken) {
        return await this.generateAccessTokenByRefreshTokenAsync("", refreshToken);
      } else {
        await this.router.navigate(["/log-in"]);
        return;
      }
    }
  }
}
