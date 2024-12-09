// services/dialog.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { DialogData } from './dialog';

@Injectable({
  providedIn: 'root',
})
export class DialogService {
  private dialogSubject = new BehaviorSubject<DialogData | null>(null);
  private resolveRef: ((value: boolean) => void) | null = null;

  show(data: DialogData): Promise<boolean> {
    this.dialogSubject.next(data);
    return new Promise((resolve) => {
      this.resolveRef = resolve;
    });
  }

  close(result: boolean = false) {
    if (this.resolveRef) {
      this.resolveRef(result);
      this.resolveRef = null;
    }
    this.dialogSubject.next(null);
  }

  getDialog(): Observable<DialogData | null> {
    return this.dialogSubject.asObservable();
  }

  // Utility methods
  showError(message: string): Promise<boolean> {
    return this.show({
      title: 'Error',
      message,
      type: 'error',
      confirmText: 'OK',
    });
  }

  showConfirm(message: string): Promise<boolean> {
    return this.show({
      title: 'Confirm',
      message,
      type: 'confirm',
      confirmText: 'Yes',
      cancelText: 'No',
    });
  }
  showInfo(message: string): Promise<boolean> {
    return this.show({
      title: 'Info',
      message,
      type: 'success',
      confirmText: 'OK',
    });
  }
}
