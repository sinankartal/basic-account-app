import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Token } from 'src/model/models';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  login(username: string, password: string): Observable<Token> {
    const body = { username, password };
    return this.http.post<Token>(
      this.apiUrl + `Token/login?username=${username}&password=${password}`,
      {},
      this.httpOptions
    );
  }
}
