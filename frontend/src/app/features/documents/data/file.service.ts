import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, Observable, tap } from 'rxjs';
import { FileUploadDto } from './file';
import { environment } from '../../../../environments/environment';
import { DialogService } from '../../../shared/ui/dialog/data/dialog.service';
import { LoginService } from '../../login/data/login.service';
@Injectable({
  providedIn: 'root',
})
export class FileService {
  private apiUrl = `${environment.apiUrl}/api/files`;

  constructor(private http: HttpClient, private dialogService : DialogService
    ,private loginService : LoginService
  ) {}

  uploadFile(file: File, name: string): Observable<FileUploadDto> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('name', name);
    formData.append('scrapedAt', new Date().toISOString());

    return this.http.post<FileUploadDto>(this.apiUrl, formData).pipe(
      tap((response) => this.dialogService.showInfo('File uploaded successfully')),
      catchError((error) => {
      this.dialogService.showError('Error uploading file');
      throw error;
      })
    );
  }

  getAllFilesByUser(): Observable<FileUploadDto[]> {
    return this.http.get<FileUploadDto[]>(`${this.apiUrl}/user/${this.loginService.getIdUserLoggedIn()}`);
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
