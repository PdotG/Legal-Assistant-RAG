import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { HttpClient, provideHttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { isPlatformBrowser } from '@angular/common';
import { TokenTimerService } from './token-timer.service';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private apiUrl = environment.apiUrl;

  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  public isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object,
    private tokenTimerService: TokenTimerService
  ) {
    if (isPlatformBrowser(this.platformId)) {
      this.isLoggedInSubject.next(this.hasToken());
    }
  }

  private hasToken(): boolean {
    return !!localStorage.getItem('token');
  }

  login(email: string, password: string): Observable<any> {
    return this.http
      .post<any>(`${this.apiUrl}/api/auth/login`, { email, password })
      .pipe(
        tap((response) => {
          if (response.token && isPlatformBrowser(this.platformId)) {
            localStorage.setItem('token', response.token);
            this.isLoggedInSubject.next(true);

            const payload = JSON.parse(atob(response.token.split('.')[1]));
            const expirationTime = payload.exp * 1000 - Date.now(); 
            this.tokenTimerService.startLogoutTimer(expirationTime);
          }
        })
      );
  }

  logout(): void {
    this.tokenTimerService.clearLogoutTimer();
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('token');
      this.isLoggedInSubject.next(false);
    }
  }

  getToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem('token');
    }
    return null;
  }

  isLoggedIn(): boolean {
    if (isPlatformBrowser(this.platformId)) {
      return this.hasToken();
    }
    return false;
  }

  getIdUserLoggedIn(): string | null {
    const token = this.getToken();
    if (token) {
      const payload = token.split('.')[1];
      const decodedPayload = atob(payload.replace('-', '+').replace('_', '/'));
      const parsedPayload = JSON.parse(decodedPayload);
      return parsedPayload.sub || null;
    }
    return null;
  }
}
