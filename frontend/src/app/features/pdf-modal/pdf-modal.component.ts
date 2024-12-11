import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject } from '@angular/core';
import { MatDialogModule, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgxExtendedPdfViewerModule } from 'ngx-extended-pdf-viewer';

@Component({
  selector: 'app-pdf-modal',
  imports: [CommonModule, MatDialogModule, NgxExtendedPdfViewerModule],
  templateUrl: './pdf-modal.component.html',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PdfModalComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public pdfSrc: string) {
    console.log('PDF URL recibida:', pdfSrc);
  }
}