import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, provideHttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { Register } from './register';
import { RegisterSuccessResponse } from './register-response';
@Injectable({
  providedIn: 'root',
})
export class RegisterService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  register(user: Register): Observable<RegisterSuccessResponse> {
    return this.http.post<RegisterSuccessResponse>(`${this.apiUrl}/api/users`, user).pipe(
      map(response => {
        return response;
      }),
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Ocurrió un error desconocido.';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      if (error.status === 409) {
        errorMessage = error.error.message || 'El correo electrónico ya está en uso.';
      } else if (error.status === 500) {
        errorMessage = error.error.message || 'Error interno del servidor.';
      } else {
        errorMessage = error.error.message || `Error ${error.status}: ${error.statusText}`;
      }
    }
    return throwError(() => new Error(errorMessage));
  }

}
