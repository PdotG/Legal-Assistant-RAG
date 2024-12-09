import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ChatRequestDto } from './chat';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private apiUrl = `${environment.apiUrl}/api/chat`;

  constructor(private http: HttpClient) {}

  askQuestion(message: string, fileId: number): Observable<string> {
    return new Observable<string>((observer) => {
      const requestBody: ChatRequestDto = {
        message: message,
        fileId: fileId
      };

      // Realizar POST con el cuerpo de la solicitud
      this.http.post(`${this.apiUrl}/ask`, requestBody, {
        responseType: 'text',
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      }).subscribe();

      // Configurar EventSource para recibir el streaming
      const eventSource = new EventSource(
        `${this.apiUrl}/ask`,
        { withCredentials: true }
      );

      // Manejar mensajes recibidos
      eventSource.onmessage = (event) => {
        if (event.data === '[DONE]') {
          observer.complete();
          eventSource.close();
        } else {
          observer.next(event.data);
        }
      };

      // Manejar errores
      eventSource.onerror = (error) => {
        observer.error(error);
        eventSource.close();
      };

      // Limpieza al desuscribirse
      return () => {
        eventSource.close();
      };
    });
  }
}
