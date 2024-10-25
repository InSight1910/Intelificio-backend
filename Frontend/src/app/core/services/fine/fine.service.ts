import { Injectable } from '@angular/core';
import {environment} from "../../../../environments/environment.development";
import {HttpClient, HttpResponse} from "@angular/common/http";
import {Observable} from "rxjs";
import {
  AssignedFine,
  AssignFineData,
  CreateAssignedFine,
  CreateFine,
  Fine, UpdateAssignedFine,
  UpdateFine
} from "../../../shared/models/fine.model";

@Injectable({
  providedIn: 'root'
})
export class FineService {

  private apiUrl: string = environment.apiUrl;
  constructor(private http: HttpClient) { }

  createFine(createFine : CreateFine): Observable<HttpResponse<any>> {
    return this.http.post<any>(`${this.apiUrl}/Fine`,createFine,{ observe: 'response' })
  }

  deleteFine(fineId: number): Observable<HttpResponse<any>>{
    return this.http.delete<any>(`${this.apiUrl}/Fine/${fineId}`,{ observe: 'response' })
  }

  updateFine(fineId: number,updateFine: UpdateFine): Observable<HttpResponse<any>>{
    return this.http.put<any>(`${this.apiUrl}/Fine/${fineId}`,updateFine,{ observe: 'response' })
  }

  getAllFinesbyCommunityId(communityId: number): Observable<{data: Fine[]}>{
    return this.http.get<{data: Fine[]}>(`${this.apiUrl}/Fine/GetAllByCommunity/${communityId}`)
  }

  getFineById(fineId: number): Observable<Fine>{
    return this.http.get<Fine>(`${this.apiUrl}/Fine/GetById/${fineId}`)
  }

  // AssignedFines

  createAssignedFine(assignedFine : CreateAssignedFine): Observable<HttpResponse<any>> {
    return this.http.post<any>(`${this.apiUrl}/AssignedFines`,assignedFine,{ observe: 'response' })
  }

  deleteAssignedFine(assignedFineId: number): Observable<HttpResponse<any>>{
    return this.http.delete<any>(`${this.apiUrl}/AssignedFines/${assignedFineId}`,{ observe: 'response' })
  }

  updateAssignedFine(assignedFineId: number,updateAssignedFine: UpdateAssignedFine): Observable<HttpResponse<any>>{
    return this.http.put<any>(`${this.apiUrl}/AssignedFines/${assignedFineId}`,updateAssignedFine,{ observe: 'response' })
  }

  getAssignedFineById(assignedFineId: number): Observable<AssignFineData>{
    return this.http.get<AssignFineData>(`${this.apiUrl}/AssignedFines/${assignedFineId}`)
  }

  getAssignedFinesByUnitId(unitId: number): Observable<{data: AssignFineData[]}>{
    return this.http.get<{data: AssignFineData[]}>(`${this.apiUrl}/AssignedFines/GetByUnit/${unitId}`)
  }

  getAssignedFinesByUserId(userId: number): Observable<{data: AssignFineData[]}>{
    return this.http.get<{data: AssignFineData[]}>(`${this.apiUrl}/AssignedFines/GetByUser/${userId}`)
  }

  getAllAssignedFinesbyCommunityId(communityId: number): Observable<{data: AssignFineData[]}>{
    return this.http.get<{data: AssignFineData[]}>(`${this.apiUrl}/AssignedFines/GetByCommunity/${communityId}`)
  }



}
