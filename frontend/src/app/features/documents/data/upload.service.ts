import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FileUploadDto } from './file';
import { environment } from '../../../../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class UploadService {
  private apiUrl = `${environment.apiUrl}/api/files`;

  constructor(private http: HttpClient) {}

  uploadFile(file: File, name: string): Observable<FileUploadDto> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('name', name);
    formData.append('scrapedAt', new Date().toISOString());

    return this.http.post<FileUploadDto>(this.apiUrl, formData);
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

  downloadFile(id: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/download/${id}`, {
      responseType: 'blob',
      headers: new HttpHeaders().append('Accept', 'application/pdf')
    });
  }
}
