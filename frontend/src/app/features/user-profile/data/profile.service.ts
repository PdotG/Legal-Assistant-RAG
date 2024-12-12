import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { LoginService } from '../../login/data/login.service';
import { Profile } from './profile';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient, private loginService: LoginService) {}

  getUserProfile(): Observable<any> {
    const userId = this.loginService.getIdUserLoggedIn();
    return this.http.get<any>(`${this.apiUrl}/api/users/${userId}`);
  }

  updateUserProfile(user: Profile): Observable<any> {
    const userId = this.loginService.getIdUserLoggedIn();
    return this.http.put<any>(`${this.apiUrl}/api/users/${userId}`, user);
  }


}