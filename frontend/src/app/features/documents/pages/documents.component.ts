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
  activeDocument: FileUploadDto | undefined = undefined;
  isUploading = false;

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

    this.isUploading = true;
    this.fileService
      .uploadFile(this.selectedFile, this.selectedFile.name)
      .subscribe({
        next: (response) => {
          this.dialogService.showInfo('File uploaded successfully');
          this.loadDocuments();
          const fileInput = document.querySelector('#file') as HTMLInputElement;
          if (fileInput) fileInput.value = '';
          this.selectedFile = null;
          this.selectedFileName = null;
          form.resetForm();
          this.isUploading = false;
        },
        error: (error) => {
          console.error('Error uploading file:', error);
          this.dialogService.showError('Error uploading file');
          this.isUploading = false;
        },
      });
  }

  async deleteDocument(name: string): Promise<void> {
    const confirmed = await this.dialogService.showConfirm(
      'Are you sure you want to delete this client?'
    );
    if (!confirmed) return;
    this.fileService.deleteFile(name).subscribe({
      next: () => {
        this.loadDocuments();
        this.dialogService.showInfo('Document has been deleted succesfully');
      },
      error: (error) => this.dialogService.showError('Error deleting document'),
    });
  }

  openChat(document: FileUploadDto): void {
    this.activeDocument = document;
    this.isChatModalVisible = true;

    if (this.chatModal) {
      this.chatModal.resetChat();
    }
  }

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
