export interface User {
  sub: number;
  email: string;
  given_name: string;
  role: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
}

export interface UserRut {
  id: number;
  name: string;
  role: string;
}

export interface AuthState {
  user: User | null;
  loading: boolean;
  error: string[] | null;
}

// export type UserAssign = Pick<User, 'given_name' | 'role'> & { rut: string;};


