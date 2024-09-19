export interface Login {
  token: string;
  refreshToken: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}
