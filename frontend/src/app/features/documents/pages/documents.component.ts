import { Component, OnInit } from '@angular/core';
import { UploadService } from '../data/upload.service';
import { FileUploadDto } from '../data/file';
import { NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ViewChild, ElementRef } from '@angular/core';
import { ChatModalComponent } from "../../chat-modal/pages/chat-modal.component";
import { MatDialog } from '@angular/material/dialog';
import { PdfModalComponent } from '../../pdf-modal/pdf-modal.component';

@Component({
  selector: 'app-documents',
  imports: [CommonModule, FormsModule, ChatModalComponent],
  templateUrl: './documents.component.html',
  styleUrls: ['./documents.component.css']
})
export class DocumentsComponent implements OnInit {
  @ViewChild('fileInput') fileInput!: ElementRef;
  @ViewChild(ChatModalComponent) chatModal!: ChatModalComponent;

  documents: FileUploadDto[] = [];
  selectedFile: File | null = null;

  isChatModalVisible = false;
  // Documento seleccionado para el chat
  activeDocument: FileUploadDto | undefined = undefined;

  constructor(private uploadService: UploadService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadDocuments();
  }

  loadDocuments(): void {
    this.uploadService.getAllFiles().subscribe({
      next: (files) => this.documents = files,
      error: (error) => console.error('Error loading documents:', error)
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file && file.type === 'application/pdf') {
      this.selectedFile = file;
    } else {
      alert('Please select a PDF file');
      event.target.value = null;
    }
  }

  onUpload(form: NgForm): void {
    if (!this.selectedFile) {
      alert('Please select a file');
      return;
    }

    this.uploadService.uploadFile(this.selectedFile, this.selectedFile.name)
      .subscribe({
        next: (response) => {
          console.log('File uploaded successfully', response);
          this.loadDocuments();
          form.resetForm();
          this.selectedFile = null;
          this.fileInput.nativeElement.value = '';
        },
        error: (error) => {
          console.error('Error uploading file:', error);
          alert('Error uploading file');
        }
      });
  }

  deleteDocument(name: string): void {
    this.uploadService.deleteFile(name).subscribe({
      next: () => {
        this.documents = this.documents.filter(doc => doc.name !== name);
      },
      error: (error) => console.error('Error deleting document:', error)
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

  openPdfModal(documentId: number): void {
    this.uploadService.getFileById(documentId).subscribe({
      next: (file: FileUploadDto) => {
        const pdfSrc = file.filePath;
        this.dialog.open(PdfModalComponent, {
          data: pdfSrc,
          width: '80%',
          height: '90%'
        });
      },
      error: (error) => {
        console.error('Error obteniendo el archivo:', error);
      }
    });
  }
}
