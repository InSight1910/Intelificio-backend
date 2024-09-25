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
  constructor(private http: HttpClient) {}
  baseUrl = environment.apiUrl;

  getUnitsByBuilding(buildingId: number) {
    return this.http.get<{ data: Unit[] }>(
      `${this.baseUrl}/unit/GetAllByBuilding/${buildingId}`
    );
  }

  getTypes() {
    return this.http.get<{ data: UnitType[] }>(`${this.baseUrl}/unit/Types`);
  }

  updateUnit(action: UpdateUnit) {
    return this.http.put(`${this.baseUrl}/unit/${action.id}`, action);
  }

  createUnit(unit: CreateUnit) {
    return this.http.post(`${this.baseUrl}/unit`, unit);
  }

  getById(id: number): Observable<{ data: Unit }> {
    return this.http.get<{ data: Unit }>(`${this.baseUrl}/unit/GetByID/${id}`);
  }
}
