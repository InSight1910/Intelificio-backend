import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Expense } from '../../../shared/models/expense.model';

@Injectable({
  providedIn: 'root',
})
export class ExpenseService {
  http: HttpClient = inject(HttpClient);
  baseUrl: string = `${environment.apiUrl}/expense`;

  create(expense: Expense) {
    return this.http.post<Expense>(this.baseUrl, expense);
  }
}
