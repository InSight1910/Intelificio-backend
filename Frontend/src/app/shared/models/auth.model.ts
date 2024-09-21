export interface Login {
  data: {
    token: string;
    refreshToken: string;
  };
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface UserEmail {
  id: number;
  name: string;
  role: string;
  phoneNumber: string;
}
