import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User, UserAccountsDTO } from '../model/models';
@Injectable({
  providedIn: 'root',
})
export class AccountService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  token = localStorage.getItem('token');

  create(userId: string, initialAmount: number): Observable<string> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.token}`
    );

    return this.http.post<string>(
      this.apiUrl + 'account/create',
      { userId: userId, initialAmount: initialAmount },
      { headers }
    );
  }

  addTransaction(accountId: string, amount: number): Observable<string> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.token}`
    );

    return this.http.post<string>(
      this.apiUrl + 'account/add-transaction',
      { accountId: accountId, amount: amount },
      { headers }
    );
  }
}
