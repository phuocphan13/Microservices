export class LoginResponse {
  accessToken: string | undefined;
  accessTokenExpires: Date | undefined;
  tokenType: string | undefined;
  refreshToken: string | undefined;
  refreshTokenExpires: Date | undefined;
}
