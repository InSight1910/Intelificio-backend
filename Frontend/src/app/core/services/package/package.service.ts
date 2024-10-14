import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { CreatePackage, Package } from '../../../shared/models/package.model';
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

  markAsDelivered(id: number, deliveredToId: number) {
    return this.http.put<Responses<Package>>(
      `${this.baseUrl}/delivered/${id}/${deliveredToId}`,
      {}
    );
  }
}
