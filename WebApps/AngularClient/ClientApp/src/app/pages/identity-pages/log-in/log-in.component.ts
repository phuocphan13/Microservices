import { Component } from '@angular/core';
import { LoginRequest } from "../../../core/models/identity/login-request.model";
import { IdentityService } from "../../../core/service/identity/identity.service";
import { CookiesService } from "../../../core/service/shared/cookie.service";
import { Router } from "@angular/router";
import { Token } from "../../../common/const";

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrl: './log-in.component.css'
})
export class LogInComponent {
  request: LoginRequest = new LoginRequest();

  constructor(private identityService: IdentityService, private cookService: CookiesService, private router: Router) {
    let accessToken = this.cookService.getCookie("AccessToken");

    if (accessToken)
    {
       this.router.navigate(["/home"]).then();
    }

    this.request.userName = "Admin";
    this.request.password = "Ab123456_";
  }

  async onClickSignIn() {
    let result = await this.identityService.login(this.request);

    this.cookService.setCookie(Token.AccessToken, result.accessToken!, result.accessTokenExpires!);
    this.cookService.setCookie(Token.RefreshToken, result.refreshToken!, result.refreshTokenExpires!)
    this.cookService.setCookie(Token.TokenType, result.tokenType!, result.accessTokenExpires!);

    await this.router.navigate(["/home"]);
  }
}
