import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { CreateGuest, UpdateGuest, Guest  } from '../../../shared/models/guest.model';

@Injectable({
  providedIn: 'root'
})
export class GuestService {
  constructor(private http: HttpClient) {}
  baseUrl = environment.apiUrl;

  // Command: Create
  createGuest(guest: CreateGuest) {
    return this.http.post(`${this.baseUrl}/guest/Create`, guest);
  }

  // Command: Update
  updateGuest(guestId: number, action: UpdateGuest) {
    return this.http.put(`${this.baseUrl}/guest/Update/${action.unitId}`, action);
  }

  // Command: Delete
  deleteGuest(id: number) {
    return this.http.delete(`${this.baseUrl}/guest/Delete/${id}`);
  }

  // Query: GetById
  getGuestById(id: number) {
    return this.http.get(`${this.baseUrl}/guest/GetById/${id}`);
  }

  // Query: GetAllByUnit
  getAllGuestsByUnit(unitId: number) {
    return this.http.get<{ data: Guest[] }>(
      `${this.baseUrl}/guest/GetAllByUnit/${unitId}`);
  }

  // Query: GetAllByEntryTime
  getAllGuestsByEntryTime(entryTime: Date) {
    return this.http.get(`${this.baseUrl}/guest/GetAllByEntryTime/${entryTime}`);
  }

}
