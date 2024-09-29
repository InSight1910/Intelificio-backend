import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import {
  CommonSpace,
  CreateCommonSpace,
} from '../../../shared/models/commonspace.model';
import { Response } from '../../../shared/models/response.model';

@Injectable({
  providedIn: 'root',
})
export class CommonSpaceService {
  baseUrl: string = `${environment.apiUrl}/commonspace`;
  constructor(private http: HttpClient) {}

  getCommonSpace(id: number) {
    return this.http.get<Response<CommonSpace>>(`${this.baseUrl}/${id}`);
  }

  getCommonSpacesByCommunity(id: number) {
    return this.http.get<Response<CommonSpace[]>>(
      `${this.baseUrl}/community/${id}`
    );
  }

  createCommonSpace(data: CreateCommonSpace) {
    return this.http.post(`${this.baseUrl}`, data);
  }

  updateCommonSpace(id: number, data: CreateCommonSpace) {
    return this.http.put(`${this.baseUrl}/${id}`, data);
  }

  deleteCommonSpace(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}