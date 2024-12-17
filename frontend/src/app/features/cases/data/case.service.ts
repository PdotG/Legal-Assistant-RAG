import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Case } from './case';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CaseService {
  private apiUrl = `${environment.apiUrl}/api/cases`;

  constructor(private http: HttpClient) {}

  getAllCasesByIdUser(userId: number): Observable<Case[]> {
    return this.http.get<Case[]>(`${this.apiUrl}/user/${userId}`);
  }

  deleteCase(idCase: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${idCase}`);
  }

  // ...otros métodos necesarios...
}