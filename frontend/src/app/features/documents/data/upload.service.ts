import { Injectable } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { FileUploadDto } from './file';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UploadService {
  private apiUrl = `${environment.apiUrl}/api/files`;
  private uploadProgress = new BehaviorSubject<number>(0);
  uploadProgress$ = this.uploadProgress.asObservable();

  constructor(private http: HttpClient) {}

  uploadFile(file: File, name: string): Observable<FileUploadDto> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('name', name);
    formData.append('scrapedAt', new Date().toISOString());

    return this.http
      .post<FileUploadDto>(this.apiUrl, formData, {
        reportProgress: true,
        observe: 'events',
      })
      .pipe(
        tap((event) => {
          if (event.type === HttpEventType.UploadProgress) {
            const progress = Math.round(
              100 * (event.loaded / (event.total || 1))
            );
            this.uploadProgress.next(progress);
          }
        }),
        map((event) => {
          if (event.type === HttpEventType.Response) {
            return event.body as FileUploadDto;
          }
          return null;
        })
      );
  }

  getAllFiles(): Observable<FileUploadDto[]> {
    return this.http.get<FileUploadDto[]>(this.apiUrl);
  }

  getFileById(id: number): Observable<FileUploadDto> {
    return this.http.get<FileUploadDto>(`${this.apiUrl}/${id}`);
  }

  deleteFile(name: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${name}`);
  }
}
