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
  fullName: string;
  firstName: string;
  lastName: string;
  role: string;
  phoneNumber: string;
}

export interface UpdateUser {
  firstName: string;
  lastName: string;
  phoneNumber: string;
  email: string;
  token: string;
  refreshToken: string;
}

export interface ConfirmEmail{
  email: string;
  token: string;
}


