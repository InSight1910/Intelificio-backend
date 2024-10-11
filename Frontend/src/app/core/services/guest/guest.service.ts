import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { 
  CreateGuest, 
  UpdateGuest, 
  Guest  
} from '../../../shared/models/guest.model';

@Injectable({
  providedIn: 'root',
})
export class GuestService {
  constructor(private http: HttpClient) {}
  baseUrl = environment.apiUrl;

  // CREATE
  createGuest(guest: CreateGuest) {
    return this.http.post(`${this.baseUrl}/guest`, guest);
  }

  // UPDATE
  updateGuest(action: UpdateGuest) {
    return this.http.put(`${this.baseUrl}/guest/${action.id}`, action);
  }

  // DELETE
  deleteGuest(id: number) {
    return this.http.delete(`${this.baseUrl}/guest/Delete/${id}`);
  }

  // BY ID
  getGuestById(id: number) {
    return this.http.get<{data: Guest}>(`${this.baseUrl}/guest/GetById/${id}`);
  }

  // ALL BY UNIT
  getAllGuestsByUnit(unitId: number) {
    return this.http.get<{ data: Guest[] }>(
      `${this.baseUrl}/guest/GetAllByUnit/${unitId}`);
  }

  // ALL BY ENTRY TIME
  getAllGuestsByEntryTime(entryTime: Date) {
    return this.http.get(`${this.baseUrl}/guest/GetAllByEntryTime/${entryTime}`);
  }

  // ALL BY COMMUNITY
  getAllGuestsByCommunity(communityId: number) {
    return this.http.get<{ data: Guest[] }>(`${this.baseUrl}/guest/GetAllByCommunity/${communityId}`);
  }

}
