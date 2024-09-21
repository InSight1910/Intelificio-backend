import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import {
  Community,
  UsersCommunity,
} from '../../../shared/models/community.model';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CommunityService {
  constructor(private http: HttpClient) {}

  baseUrl = environment.apiUrl;

  getCommunitiesOfUser(userId: number) {
    var result = this.http
      .get<{ data: Community[] }>(`${this.baseUrl}/community/user/${userId}`)
      .pipe(map((result) => result.data));
    return result;
  }

  updateCommunity(action: Community) {
    return this.http.put(`${this.baseUrl}/community/${action.id}`, action);
  }

  getCommunity(id: number) {
    return this.http.get<{ data: Community }>(
      `${this.baseUrl}/community/${id}`
    );
  }

  getUsersByCommunity(communityId: number) {
    return this.http.get<{ data: UsersCommunity[] }>(
      `${this.baseUrl}/community/${communityId}/users`
    );
  }
}
