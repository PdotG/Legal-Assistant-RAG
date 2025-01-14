import { CommonModule } from '@angular/common';
import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormsModule, NgForm } from '@angular/forms';
import { LoginService } from '../../login/data/login.service';
import { CaseService } from '../../cases/data/case.service';
import { DialogService } from '../../../shared/ui/dialog/data/dialog.service';
import { CaseRequestDto, ClientSummaryDto } from '../../create-case-modal/data/create-case-modal.interfaces';
import { ClientService } from '../../clients/data/client.service';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Component({
  selector: 'app-edit-case-modal',
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-case-modal.component.html',
  styleUrl: './edit-case-modal.component.css'
})
export class EditCaseModalComponent implements OnInit {
  case: CaseRequestDto = {
    title: '',
    description: '',
    status: 'Open',
    courtDate: undefined,
    clientId: 0,
    assignedUserId: 0
  };

  clients: ClientSummaryDto[] = [];
  caseId: number;
  clientName$: Observable<string> = of('Unknown');
  courtDateFormatted: string | undefined;

  constructor(
    private authService: LoginService,
    private caseService: CaseService,
    private clientService: ClientService,
    private dialogService: DialogService,
    public dialogRef: MatDialogRef<EditCaseModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { caseId: number, idClient: number }
  ) { 
    this.caseId = data.caseId;
    this.case.clientId = data.idClient;
    const userId = Number(this.authService.getIdUserLoggedIn());
    this.case.assignedUserId = userId;
  }

  ngOnInit(): void {
    this.loadCase();
    this.clientName$ = this.getClientName(this.case.clientId);
  }

  getClientName(id: number): Observable<string> {
    if (id === 0) {
      return of('Unknown (ID: N/A)');
    }
    return this.clientService.getClientById(id).pipe(
      map(client => `${client.idClient} - ${client.name}`),
      catchError(error => {
        this.dialogService.showError('Error fetching client name');
        console.log(error);
        return of('Unknown (ID: N/A)');
      })
    );
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    const pad = (n: number) => n < 10 ? '0' + n : n;
    const yyyy = date.getFullYear();
    const MM = pad(date.getMonth() + 1);
    const dd = pad(date.getDate());
    const hh = pad(date.getHours());
    const mm = pad(date.getMinutes());
    return `${yyyy}-${MM}-${dd}T${hh}:${mm}`;
  }

  async loadCase(): Promise<void> {
    try {
      const caseData = await this.caseService.getCaseById(this.caseId).toPromise();
      if (caseData) {
        this.case = {
          title: caseData.title,
          description: caseData.description,
          status: caseData.status,
          courtDate: this.case.clientId && caseData.courtDate ? this.formatDate(caseData.courtDate) : undefined,
          clientId: this.case.clientId,
          assignedUserId: this.case.assignedUserId 
        };
        this.courtDateFormatted = this.case.courtDate;
      } else {
        await this.dialogService.showError('Case data is undefined');
      }
    } catch (error) {
      await this.dialogService.showError('Error loading case details');
    }
  }

  async onSubmit(form: NgForm): Promise<void> {
    if (form.valid) {
      try {
        this.case.courtDate = this.courtDateFormatted ? new Date(this.courtDateFormatted).toISOString() : undefined;
        console.log(this.case);
        await this.caseService.updateCase(this.caseId, this.case).toPromise();
        this.dialogRef.close(true);
        await this.dialogService.showInfo('Case updated successfully');
      } catch (error) {
        await this.dialogService.showError('Error updating case');
        console.log(error);
      }
    }
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
