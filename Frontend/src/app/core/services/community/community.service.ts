import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import {
  Community,
  UsersCommunity,
  AllCommunity,
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
    return this.http.put<{ data: Community }>(
      `${this.baseUrl}/community/${action.id}`,
      action,
      { observe: 'response' }
    );
  }

  getCommunity(id: number) {
    return this.http.get<{ data: Community }>(
      `${this.baseUrl}/community/${id}`
    );
  }

  getAllCommunity() {
    return this.http.get<{ data: AllCommunity[] }>(`${this.baseUrl}/community`);
  }

  getUsersByCommunity(communityId: number) {
    return this.http.get<{ data: UsersCommunity[] }>(
      `${this.baseUrl}/community/${communityId}/users`
    );
  }

  addUserToCommunity(communityId: number, userId: number) {
    return this.http.put(
      `${this.baseUrl}/community/add/${communityId}/${userId}`,
      {}
    );
  }

  addUserToCommunityWithFile(formData: FormData) {
    return this.http.post(
      `${this.baseUrl}/community/add/user/massive`,
      formData
    );
  }

  deleteUserFromCommunity(communityId: number, userId: number) {
    return this.http.put(
      `${this.baseUrl}/community/remove/${communityId}/${userId}`,
      {}
    );
  }

  createCommunity(community: Community) {
    return this.http.post<{ data: Community }>(
      `${this.baseUrl}/community`,
      community
    );
  }
}
