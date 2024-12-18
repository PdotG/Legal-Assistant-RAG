import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ClientService } from '../../clients/data/client.service';
import { DialogService } from '../../../shared/ui/dialog/data/dialog.service';
import { CaseRequestDto, ClientSummaryDto, UserSummaryDto } from '../data/create-case-modal.interfaces';
import { UserService } from '../../user-profile/data/profile.service';

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
  users: UserSummaryDto[] = [];

  constructor(
    private clientService: ClientService,
    private userService: UserService, // Inyecta el servicio de usuarios
    private dialogService: DialogService,
    public dialogRef: MatDialogRef<CreateCaseModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { userId: number }
  ) { }

  ngOnInit(): void {
    this.loadClients();
    this.loadUsers(); // Carga los usuarios al inicializar
  }

  async loadClients(): Promise<void> {
    try {
      const clients = await this.clientService.getAllClientsByIdUser(1).toPromise();
      this.clients = clients ?? [];
    } catch (error) {
      await this.dialogService.showError('Error loading clients');
    }
  }

  async loadUsers(): Promise<void> {
    // try {
    //   const users = await this.userService.getUsers().toPromise();
    //   this.users = users ?? [];
    // } catch (error) {
    //   await this.dialogService.showError('Error loading users');
    // }
  }

  async onSubmit(form: NgForm): Promise<void> {
    if (form.valid) {
      // try {
      //   await this.clientService.createCase(this.case).toPromise();
      //   this.dialogRef.close(true);
      //   await this.dialogService.showInfo('Case created successfully');
      // } catch (error) {
      //   await this.dialogService.showError('Error creating case');
      // }
    }
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}

