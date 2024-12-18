import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ClientService } from '../../clients/data/client.service';
import { DialogService } from '../../../shared/ui/dialog/data/dialog.service';
import { CaseRequestDto, ClientSummaryDto } from '../data/create-case-modal.interfaces';
import { LoginService } from '../../login/data/login.service';
import { CaseService } from '../../cases/data/case.service';

@Component({
  selector: 'app-create-case-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './create-case-modal.component.html'
})
export class CreateCaseModalComponent implements OnInit {
  case: CaseRequestDto = {
    title: '',
    description: '',
    status: 'Open',
    courtDate: undefined,
    clientId: 0,
    assignedUserId: 0
  };

  clients: ClientSummaryDto[] = [];

  constructor(
    private clientService: ClientService,
    private authService: LoginService,
    private caseService: CaseService,
    private dialogService: DialogService,
    public dialogRef: MatDialogRef<CreateCaseModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { userId: number }
  ) { 
    const userId = this.authService.getIdUserLoggedIn();
    this.case.assignedUserId = userId !== null ? parseInt(userId, 10) : 0;
  }

  ngOnInit(): void {
    this.loadClients();
  }

  async loadClients(): Promise<void> {
    try {
      const clients = await this.clientService.getAllClientsByIdUser(this.case.assignedUserId).toPromise();
      this.clients = clients ?? [];
    } catch (error) {
      await this.dialogService.showError('Error loading clients');
    }
  }

  async onSubmit(form: NgForm): Promise<void> {
    if (form.valid) {
      try {
        this.case.courtDate = this.case.courtDate ? new Date(this.case.courtDate).toISOString() : undefined;
        await this.caseService.createCase(this.case).toPromise();
        this.dialogRef.close(true);
        await this.dialogService.showInfo('Case created successfully');
      } catch (error) {
        await this.dialogService.showError('Error creating case');
        console.log(error);
      }
    }
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}

