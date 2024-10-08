import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment.development';
import {
  CountReservation,
  CreateReservation,
  ListReservation,
  Reservation,
} from '../../../shared/models/reservation.model';
import { Response } from '../../../shared/models/response.model';

@Injectable({
  providedIn: 'root',
})
export class ReservationService {
  baseUrl = `${environment.apiUrl}/reservation`;

  constructor(private http: HttpClient) {}

  create(reservation: CreateReservation) {
    return this.http.post<Response<Reservation>>(
      `${this.baseUrl}`,
      reservation
    );
  }

  getReservationsByCommunityAndMonth(communityId: number, date: Date) {
    const dateToSeach = `${date.getFullYear()}-${date.getMonth()}-${date.getDate()}`;
    return this.http.get<Response<ListReservation[]>>(
      `${this.baseUrl}/community/${communityId}/${dateToSeach}`
    );
  }

  getCountReservationsByCommunityAndMonth(
    communityId: number,
    year: number,
    month: number
  ) {
    return this.http.get<Response<CountReservation[]>>(
      `${this.baseUrl}/count/${communityId}/${year}/${month}`
    );
  }

  confirmReservation(reservationId: number, token: string) {
    return this.http.post(`${this.baseUrl}/confirm`, { reservationId, token });
  }
}
