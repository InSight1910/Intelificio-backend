import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Responses } from '../../../shared/models/response.model';
import { UserRut } from '../../../shared/models/user.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl: string = `${environment.apiUrl}/user`;
  http: HttpClient = inject(HttpClient);

  getByRut(rut: string) {
    return this.http.get<Responses<UserRut>>(`${this.baseUrl}/rut/${rut}`);
  }
}
