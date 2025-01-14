import { HttpClient, HttpHeaders, HttpEventType, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { ChatRequestDto } from './chat';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private apiUrl = `${environment.apiUrl}/api/chat`;

  constructor(private http: HttpClient) {}

  askQuestion(message: string, fileId: number): Observable<string> {
    const requestBody: ChatRequestDto = {
      message,
      fileId
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'text/event-stream'
    });

    return new Observable<string>((observer) => {
      let buffer = '';
      
      const httpSubscription = this.http.post(`${this.apiUrl}/ask`, requestBody, {
        headers,
        observe: 'events',
        responseType: 'text',
        reportProgress: true
      }).pipe(
        filter(event => event.type === HttpEventType.DownloadProgress)
      ).subscribe({
        next: (event: HttpEvent<any>) => {
          if (event.type === HttpEventType.DownloadProgress) {
            const newData = (event as any).partialText;
            const newContent = newData.substring(buffer.length);
            buffer = newData;

            const chunks = newContent.split('\n\n')
              .filter((chunk: string) => chunk.startsWith('data: '))
              .map((chunk: string) => chunk.replace('data: ', ''))
              .map((chunk: string) => {
                return chunk
                  .replace(/([a-zA-Z])(\d)/g, '$1 $2') 
                  .replace(/(\d)([a-zA-Z])/g, '$1 $2');
              });

            chunks.forEach((chunk: string) => {
              if (chunk === '[DONE]') {
                observer.complete();
              } else {
                observer.next(chunk);
              }
            });
          }
        },
        error: (error) => observer.error(error),
        complete: () => observer.complete()
      });

      return () => httpSubscription.unsubscribe();
    });
  }
}
