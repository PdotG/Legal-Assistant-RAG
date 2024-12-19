import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Case } from './case';
import { environment } from '../../../../environments/environment';
import {
  CaseRequestDto,
  CaseResponseDto,
} from '../../create-case-modal/data/create-case-modal.interfaces';

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

  createCase(caseRequest: CaseRequestDto): Observable<CaseResponseDto> {
    return this.http.post<CaseResponseDto>(this.apiUrl, caseRequest);
  }

  getCaseById(idCase: number): Observable<CaseResponseDto> {
    return this.http.get<CaseResponseDto>(`${this.apiUrl}/${idCase}`);
  }

  updateCase(idCase: number, caseRequest: CaseRequestDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${idCase}`, caseRequest);
  }
}
