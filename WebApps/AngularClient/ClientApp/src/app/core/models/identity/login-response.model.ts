export class LoginResponse {
  accessToken: string | undefined;
  accessTokenExpires: number | undefined;
  tokenType: string | undefined;
  refreshToken: string | undefined;
  refreshTokenExpires: number | undefined;
}
