import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Building } from '../../shared/models/building.model';

@Injectable({
  providedIn: 'root'
})
export class BuildingService {
  private apiUrl: string = environment.apiUrl;
  constructor(private http: HttpClient) { }

  create(building : {}): Observable<HttpResponse<any>> {
    return this.http.post<any>(`${this.apiUrl}/building`,building,{ observe: 'response' });
  }

  delete(id: number): Observable<HttpResponse<any>>{
    return this.http.delete<any>(`${this.apiUrl}/building/${id}`,{ observe: 'response' })
  }

  update(id: number,building: {}): Observable<HttpResponse<any>>{
    return this.http.put<any>(`${this.apiUrl}/building/${id}`,building,{ observe: 'response' })
  }

  getById(id: number): Observable<Building>{
    return this.http.get<Building>(`${this.apiUrl}/building/GetByID/${id}`)
  }

  getbyCommunityId(id: number): Observable<{data: Building[]}>{
    return this.http.get<{data: Building[]}>(`${this.apiUrl}/building/GetAllBycommunity/${id}`)
  }

}
