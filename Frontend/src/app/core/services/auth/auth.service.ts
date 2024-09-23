import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { Login, UserEmail } from '../../../shared/models/auth.model';
import { map, Observable } from 'rxjs';
import { singUp } from '../../../shared/models/singup.model';
import { Role } from '../../../shared/models/role.model';
import { SignupDTO } from '../../../shared/models/signUpCommand.model';


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

  signup(singupDTO: SignupDTO): Observable<HttpResponse<any>> {
    return this.http.post<any>(`${this.apiUrl}/auth/signup`,singupDTO);
  }

  getAllRole(): Observable<{data: Role[]}>{
    return this.http.get<{data: Role[]}>(`${this.apiUrl}/auth/roles`);
  }

}
