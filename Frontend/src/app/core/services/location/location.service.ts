import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import {
  City,
  Municipality,
  Region,
} from '../../../shared/models/location.model';

@Injectable({
  providedIn: 'root',
})
export class LocationService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getMunicipality(id: number) {
    return this.http.get<{ data: Municipality }>(
      `${this.baseUrl}/location/municipality/${id}`
    );
  }

  getMunicipalities() {
    return this.http.get<{ data: Municipality[] }>(
      `${this.baseUrl}/location/municipality`
    );
  }

  getMunicipalitiesByCity(cityId: number) {
    return this.http.get<{ data: Municipality[] }>(
      `${this.baseUrl}/location/city/${cityId}/municipalities`
    );
  }

  getCity(id: number) {
    return this.http.get<{ data: City }>(`${this.baseUrl}/location/city/${id}`);
  }

  getCities() {
    return this.http.get<{ data: City[] }>(`${this.baseUrl}/location/city`);
  }

  getCitiesByRegion(regionId: number) {
    return this.http.get<{ data: City[] }>(
      `${this.baseUrl}/location/region/${regionId}/cities`
    );
  }

  getRegions() {
    return this.http.get<{ data: Region[] }>(`${this.baseUrl}/location/region`);
  }

  getRegion(id: number) {
    return this.http.get<{ data: Region }>(
      `${this.baseUrl}/location/region/${id}`
    );
  }
}
