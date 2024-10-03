import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import {Login, UpdateUser, UserEmail} from '../../../shared/models/auth.model';
import { map, Observable } from 'rxjs';
import { singUp } from '../../../shared/models/singup.model';
import { Role } from '../../../shared/models/role.model';
import { SignupDTO } from '../../../shared/models/signUpCommand.model';
import { UserAdmin } from '../../../shared/models/community.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<Login> {
    return this.http
      .post<Login>(`${this.apiUrl}/auth/login`, { email, password })
      .pipe(
        map((response) => {
          localStorage.setItem('token', response.data.token);
          localStorage.setItem('refreshToken', response.data.refreshToken);
          return response;
        })
      );
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('community');
    localStorage.removeItem('communityId');
  }

  getUserByEmail(email: string) {
    return this.http.post<{ data: UserEmail }>(
      `${this.apiUrl}/auth/user/byEmail`,
      { email }
    );
  }

  signup(signupDTO: SignupDTO): Observable<HttpResponse<any>> {
    return this.http.post<any>(`${this.apiUrl}/auth/signup`, signupDTO, {
      observe: 'response',
    });
  }

  updateUser(updateUser: UpdateUser): Observable<HttpResponse<any>> {
    return this.http.put<any>(`${this.apiUrl}/auth/user/update`, updateUser, {
      observe: 'response',
    });
  }

  signupMassive(formData: FormData) {
    return this.http.post(`${this.apiUrl}/auth/signup/massive`, formData, {
      observe: 'response',
    });
  }

  getAllRole(): Observable<{ data: Role[] }> {
    return this.http.get<{ data: Role[] }>(`${this.apiUrl}/auth/roles`);
  }

  getAllUserAdmin(): Observable<{ data: UserAdmin[] }> {
    return this.http.get<{ data: UserAdmin[] }>(
      `${this.apiUrl}/auth/User/admin`
    );
  }

  forgotPassword(email: string) {
    return this.http.post(
      `${this.apiUrl}/auth/change-password-one`,
      {
        email,
      },
      {
        observe: 'response',
      }
    );
  }

  changePassword(email: string, token: string, password: any) {
    return this.http.post(
      `${this.apiUrl}/auth/change-password-two`,
      {
        email,
        token,
        password,
      },
      {
        observe: 'response',
      }
    );
  }
}
