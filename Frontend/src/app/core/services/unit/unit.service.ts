import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { Unit, UnitType } from '../../../shared/models/unit.model';

@Injectable({
  providedIn: 'root'
})
export class UnitService {
  constructor(private http: HttpClient) { }
  baseUrl = environment.apiUrl;

  getUnitsByBuilding(buildingId: number) {
    return this.http
      .get<{ data: Unit[] }>(`${this.baseUrl}/unit/GetAllByBuilding/${buildingId}`);
  }

  getTypes() {
    return this.http.get<{ data: UnitType[] }>(`${this.baseUrl}/unit/Types`);
  }

  editUnit(unit: Unit) {
    return this.http.put(`${this.baseUrl}/unit/Update/${unit.id}`, unit);
  }

  createUnit(unit: Unit) {
    return this.http.post(`${this.baseUrl}/unit`, unit);
  }

}


/*
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { Unit } from '../../../shared/models/unit.model';

@Injectable({
  providedIn: 'root'
})
export class UnitService {
  constructor(private http: HttpClient) { }
  baseUrl = environment.apiUrl;

  getUnitsByBuilding(buildingId: number) {
    return this.http
      .get<{ data: Unit[] }>(`${this.baseUrl}/unit/GetAllByBuilding/${buildingId}`)
  }

  editUnit(unitId: number, updateData: Partial<Unit>) {
    return this.http.put<Unit>(
      `${this.baseUrl}/unit/Update/${unitId}`, updateData
    );
  }  
}
*/