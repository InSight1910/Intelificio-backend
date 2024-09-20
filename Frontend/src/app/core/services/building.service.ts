import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Building } from '../../shared/models/building.model';

@Injectable({
  providedIn: 'root'
})
export class BuildingService {
  private apiUrl: string = environment.apiUrl;
  constructor(private http: HttpClient) { }

  create(building : {}): Observable<Building> {
    return this.http.post<Building>(`${this.apiUrl}/building`,building);
  }

  delete(id: number): Observable<any>{
    return this.http.delete<any>(`${this.apiUrl}/building/${id}`)
  }

  update(id: number,building: {}): Observable<Building>{
    return this.http.put<Building>(`${this.apiUrl}/building/${id}`,building)
  }

  getById(id: number): Observable<Building>{
    return this.http.get<Building>(`${this.apiUrl}/building/GetByID/${id}`)
  }

  getbyCommunityId(id: number): Observable<{data: Building[]}>{
    return this.http.get<{data: Building[]}>(`${this.apiUrl}/building/GetAllBycommunity/${id}`)
  }

}
