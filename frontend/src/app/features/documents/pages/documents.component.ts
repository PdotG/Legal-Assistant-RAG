import { Component, OnInit } from '@angular/core';
import { FileService } from '../data/file.service';
import { FileUploadDto } from '../data/file';
import { NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ViewChild, ElementRef } from '@angular/core';
import { ChatModalComponent } from '../../chat-modal/pages/chat-modal.component';
import { MatDialog } from '@angular/material/dialog';
import { DialogService } from '../../../shared/ui/dialog/data/dialog.service';

@Component({
  selector: 'app-documents',
  imports: [CommonModule, FormsModule, ChatModalComponent],
  templateUrl: './documents.component.html',
  styleUrls: ['./documents.component.css'],
})
export class DocumentsComponent implements OnInit {
  @ViewChild('fileInput') fileInput!: ElementRef;
  @ViewChild(ChatModalComponent) chatModal!: ChatModalComponent;

  documents: FileUploadDto[] = [];
  selectedFile: File | null = null;
  selectedFileName: string | null = null;

  isChatModalVisible = false;
  // Documento seleccionado para el chat
  activeDocument: FileUploadDto | undefined = undefined;

  constructor(
    private fileService: FileService,
    private dialog: MatDialog,
    private dialogService: DialogService
  ) {}

  ngOnInit(): void {
    this.loadDocuments();
  }

  loadDocuments(): void {
    this.fileService.getAllFilesByUser().subscribe({
      next: (files) => (this.documents = files),
      error: (error) => this.dialogService.showError('Error loading documents'),
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file && file.type === 'application/pdf') {
      this.selectedFile = file;
      this.selectedFileName = file.name;
    } else {
      this.dialogService.showError('Please select a PDF file');
      event.target.value = null;
    }
  }

  onUpload(form: NgForm): void {
    if (!this.selectedFile) {
      this.dialogService.showError('Please select a file');
      return;
    }

    this.fileService
      .uploadFile(this.selectedFile, this.selectedFile.name)
      .subscribe({
        next: (response) => {
          this.dialogService.showInfo('File uploaded successfully');
          this.loadDocuments();
          // Clear file input managing DOM directly
          const fileInput = document.querySelector('#file') as HTMLInputElement;
          if (fileInput) fileInput.value = '';
          // Reset file selection state
          this.selectedFile = null;
          this.selectedFileName = null;
          // Reset form
          form.resetForm();
        },
        error: (error) => {
          console.error('Error uploading file:', error);
          this.dialogService.showError('Error uploading file');
        },
      });
  }

  deleteDocument(name: string): void {
    this.fileService.deleteFile(name).subscribe({
      next: () => {
        this.documents = this.documents.filter((doc) => doc.name !== name);
      },
      error: (error) => console.error('Error deleting document:', error),
    });
  }

  // Método para abrir el modal de chat
  openChat(document: FileUploadDto): void {
    this.activeDocument = document;
    this.isChatModalVisible = true;

    // Resetea el chat cada vez que se abre
    if (this.chatModal) {
      this.chatModal.resetChat(); // Llama al método de reinicio del chat
    }
  }

  // Método para cerrar el modal de chat
  closeChat(): void {
    this.isChatModalVisible = false;
    this.activeDocument = undefined;
  }

  openPdf(documentId: number): void {
    this.fileService.downloadFile(documentId).subscribe({
      next: (blob: Blob) => {
        const fileURL = URL.createObjectURL(blob);
        window.open(fileURL, '_blank', 'noopener,noreferrer');
        URL.revokeObjectURL(fileURL);
      },
      error: (error) => {
        console.error('Error descargando el archivo:', error);
      },
    });
  }
}
