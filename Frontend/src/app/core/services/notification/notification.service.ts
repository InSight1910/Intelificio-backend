import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import {ConfirmEmail} from "../../../shared/models/auth.model";

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private apiUrl: string = environment.apiUrl;
  constructor(private http: HttpClient) {}

  SendEmail(Email: {}): Observable<HttpResponse<any>> {
    return this.http.post<any>(
      `${this.apiUrl}/notification/SingleMessage`,
      Email,
      {
        observe: 'response',
      }
    );
  }

  resendNotification(packageId: number ): Observable<HttpResponse<any>>  {
    return this.http.post<any>(`${this.apiUrl}/Notification/Package/${packageId}`, {},{
      observe: 'response',
    });
  }

}
