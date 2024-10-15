import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import {
  Attendee,
  AttendeeCreate,
} from '../../../shared/models/attendee.model';
import { Responses } from '../../../shared/models/response.model';

@Injectable({
  providedIn: 'root',
})
export class AttendeeService {
  baseUrl: string = `${environment.apiUrl}/attendee`;
  constructor(private http: HttpClient) {}

  createAttendee(data: AttendeeCreate) {
    return this.http.post<Responses<Attendee>>(this.baseUrl, data);
  }

  deleteAttendee(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  getByReservationId(id: number) {
    return this.http.get<Responses<Attendee[]>>(
      `${this.baseUrl}/reservation/${id}`
    );
  }
}
