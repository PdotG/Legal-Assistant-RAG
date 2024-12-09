export interface DialogData {
    title: string;
    message: string;
    type: 'error' | 'confirm' | 'success';
    confirmText?: string;
    cancelText?: string;
  }