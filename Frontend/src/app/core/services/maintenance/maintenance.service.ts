import { Injectable } from '@angular/core';
import {environment} from "../../../../environments/environment.development";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Maintenance} from "../../../shared/models/maintenance.model";

@Injectable({
  providedIn: 'root'
})
export class MaintenanceService {

  private apiUrl: string = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getById(id: number): Observable<Maintenance>{
    return this.http.get<Maintenance>(`${this.apiUrl}/maintenance/GetByID/${id}`)
  }

  getbyCommunityId(id: number): Observable<{data: Maintenance[]}>{
    return this.http.get<{data: Maintenance[]}>(`${this.apiUrl}/maintenance/GetAllBycommunity/${id}`)
  }

}
