import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User, UserAccountsDTO } from '../model/models';
@Injectable({
  providedIn: 'root',
})
export class UserService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  token = localStorage.getItem('token');

  getUserList(): Observable<User[]> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.token}`
    );

    return this.http.get<User[]>(this.apiUrl + 'user/', { headers });
  }

  getUserAccounts(userId: string): Observable<UserAccountsDTO> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.token}`
    );

    return this.http.get<UserAccountsDTO>(this.apiUrl + 'user/' + userId + '/accounts', {
      headers,
    });
  }
}
