import { Injectable } from '@angular/core';
import {environment} from "../../../../environments/environment.development";
import {HttpClient, HttpResponse} from "@angular/common/http";
import {Observable} from "rxjs";
import {Contact} from "../../../shared/models/contact.model";
import {Building} from "../../../shared/models/building.model";

@Injectable({
  providedIn: 'root'
})
export class ContactService {

  private apiUrl: string = environment.apiUrl;
  constructor(private http: HttpClient) { }

  create(contact : {}): Observable<HttpResponse<any>> {
    return this.http.post<any>(`${this.apiUrl}/contact`,contact,{ observe: 'response' })
  }

  getbyCommunityId(id: number): Observable<{data: Contact[]}>{
    return this.http.get<{data: Contact[]}>(`${this.apiUrl}/contact/GetAllByCommunity/${id}`)
  }
}
