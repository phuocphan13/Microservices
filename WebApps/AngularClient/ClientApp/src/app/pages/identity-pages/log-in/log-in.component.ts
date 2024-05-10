import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginRequest } from "../../../core/models/identity/login-request.model";
import { IdentityService } from "../../../core/service/identity/identity.service";

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrl: './log-in.component.css'
})
export class LogInComponent {
  request: LoginRequest = new LoginRequest();

  constructor(private identityService: IdentityService) {
  }

  async onClickSignIn() {
    let result = await this.identityService.login(this.request);
  }
}
