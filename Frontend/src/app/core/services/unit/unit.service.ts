import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import {
  CreateUnit,
  UpdateUnit,
  Unit,
  UnitType,
} from '../../../shared/models/unit.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UnitService {
  constructor(private http: HttpClient){}
  baseUrl = environment.apiUrl;

  createUnit(unit: CreateUnit) {
    return this.http.post(`${this.baseUrl}/unit`, unit);
  }

  updateUnit(action: UpdateUnit) {
    return this.http.put(`${this.baseUrl}/unit/${action.id}`, action);
  }

  getById(id: number): Observable<{ data: Unit }> {
    return this.http.get<{ data: Unit }>(`${this.baseUrl}/unit/GetByID/${id}`);
  }

  getUnitsByBuilding(buildingId: number) {
    return this.http.get<{ data: Unit[] }>(
      `${this.baseUrl}/unit/GetAllByBuilding/${buildingId}`
    );
  }

  getTypes() {
    return this.http.get<{ data: UnitType[] }>(`${this.baseUrl}/unit/Types`);
  }

  getDeptosByBuilding(id: number) {
    return this.http.get<{ data: Unit[] }>(`${this.baseUrl}/unit/GetDeptosByBuilding/${id}`);
  }
}
