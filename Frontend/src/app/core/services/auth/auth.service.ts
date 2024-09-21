import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { Login, UserEmail } from '../../../shared/models/auth.model';
import { map, Observable } from 'rxjs';

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
  }

  getUserByEmail(email: string) {
    return this.http.post<{ data: UserEmail }>(
      `${this.apiUrl}/auth/user/byEmail`,
      { email }
    );
  }
}
