import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ClientService } from '../../clients/data/client.service';
import { DialogService } from '../../../shared/ui/dialog/data/dialog.service';
import { ClientCreate } from '../../clients/data/client-create';

@Component({
  selector: 'app-create-client-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './create-client-modal.component.html'
})
export class CreateClientModalComponent {
  client: ClientCreate = {
    idUser: 0,
    name: '',
    contactInformation: '',
    address: '',
    notes: ''
  };

  constructor(
    private clientService: ClientService,
    private dialogService: DialogService,
    public dialogRef: MatDialogRef<CreateClientModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { userId: number }
  ) {
    this.client.idUser = this.data.userId;
  }

  async onSubmit(form: NgForm): Promise<void> {
    if (form.valid) {
      try {
        await this.clientService.createClient(this.client).toPromise();
        this.dialogRef.close(true);
        await this.dialogService.showInfo('Client created successfully');
      } catch (error) {
        await this.dialogService.showError('Error creating client');
      }
    }
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
