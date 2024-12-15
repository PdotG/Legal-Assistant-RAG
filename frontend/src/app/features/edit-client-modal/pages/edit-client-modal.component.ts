import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ClientService } from '../../clients/data/client.service';
import { DialogService } from '../../../shared/ui/dialog/data/dialog.service';
import { Client } from '../../clients/data/client';

@Component({
  selector: 'app-edit-client-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './edit-client-modal.component.html'
})
export class EditClientModalComponent {
  client: Client;

  constructor(
    private clientService: ClientService,
    private dialogService: DialogService,
    public dialogRef: MatDialogRef<EditClientModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { client: Client }
  ) {
    this.client = { ...data.client };
  }

  async onSubmit(form: NgForm): Promise<void> {
    if (form.valid) {
      try {
        await this.clientService.updateClient(this.client.idClient, this.client).toPromise();
        this.dialogRef.close(true);
        await this.dialogService.showInfo('Client updated successfully');
      } catch (error) {
        await this.dialogService.showError('Error updating client');
      }
    }
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
