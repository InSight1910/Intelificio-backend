export interface User {
  sub: number;
  email: string;
  given_name: string;
  role: string;
}

export interface AuthState {
  user: User | null;
  loading: boolean;
  error: string[] | null;
}
