import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Client } from './client';
import { ClientCreate } from './client-create';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ClientService {
  private apiUrl = `${environment.apiUrl}/api/clients`;

  constructor(private http: HttpClient) {}

  getAllClientsByIdUser(idUser: number): Observable<Client[]> {
    return this.http.get<Client[]>(`${this.apiUrl}/user/${idUser}`);
  }

  getClientById(id: number): Observable<Client> {
    return this.http.get<Client>(`${this.apiUrl}/${id}`);
  }

  createClient(client: ClientCreate): Observable<any> {
    return this.http.post(`${this.apiUrl}`, client);
  }

  updateClient(id: number, client: Client): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, client);
  }

  deleteClient(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

}
