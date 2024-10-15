import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import {
  CreatePackage,
  MyPackages,
  Package,
} from '../../../shared/models/package.model';
import { Responses } from '../../../shared/models/response.model';

@Injectable({
  providedIn: 'root',
})
export class PackageService {
  http: HttpClient = inject(HttpClient);
  baseUrl: string = `${environment.apiUrl}/package`;

  create(data: CreatePackage) {
    return this.http.post<Responses<Package>>(this.baseUrl, data);
  }

  getPackages(communityId: number) {
    return this.http.get<Responses<Package[]>>(
      `${this.baseUrl}/community/${communityId}`
    );
  }

  getMyPackages(communityId: number, userId: number) {
    return this.http.get<Responses<MyPackages[]>>(
      `${this.baseUrl}/GetMyPackages/${communityId}/${userId}`
    );
  }

  markAsDelivered(id: number, deliveredToId: number) {
    return this.http.put(
      `${this.baseUrl}/markAsDelivered/${id}/${deliveredToId}`,
      {}
    );
  }
}
