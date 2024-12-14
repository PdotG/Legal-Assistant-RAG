import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClientService } from '../data/client.service';
import { DialogService } from '../../../shared/ui/dialog/data/dialog.service';
import { LoginService } from '../../login/data/login.service';
import { Client } from '../data/client';

@Component({
  selector: 'app-clients',
  imports: [CommonModule, FormsModule],
  templateUrl: './clients.component.html',
  styleUrl: './clients.component.css'
})
export class ClientsComponent implements OnInit {
  clients: Client[] = [];
  filteredClients: Client[] = [];
  searchTerm: string = '';
  loading: boolean = false;
  currentUserId: number = 0;

  constructor(
    private clientService: ClientService,
    private dialogService: DialogService,
    private authService: LoginService
  ) {
    const userId = this.authService.getIdUserLoggedIn();
    this.currentUserId = userId !== null ? parseInt(userId, 10) : 0;
  }

  ngOnInit(): void {
    this.loadClients();
  }

  async loadClients(): Promise<void> {
    try {
      this.loading = true;
      this.clients = (await this.clientService.getAllClientsByIdUser(this.currentUserId).toPromise()) || [];
      this.filteredClients = [...this.clients];
    } catch (error) {
      console.error('Error loading clients:', error);
      // Aquí podrías implementar un servicio de notificaciones
    } finally {
      this.loading = false;
    }
  }

  filterClients(): void {
    if (!this.searchTerm) {
      this.filteredClients = [...this.clients];
      return;
    }

    this.filteredClients = this.clients.filter(client =>
      client.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      client.contactInformation.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }

  async openCreateDialog(): Promise<void> {
    // Implementar diálogo de creación con Material Dialog
    // const dialogRef = this.dialog.open(CreateClientDialogComponent, {
    //   width: '500px',
    //   data: { userId: this.currentUserId }
    // });

    // dialogRef.afterClosed().subscribe(result => {
    //   if (result) {
    //     this.loadClients();
    //   }
    // });
  }

  async editClient(client: Client): Promise<void> {
    // Implementar diálogo de edición con Material Dialog
    // const dialogRef = this.dialog.open(EditClientDialogComponent, {
    //   width: '500px',
    //   data: client
    // });

    // dialogRef.afterClosed().subscribe(result => {
    //   if (result) {
    //     this.loadClients();
    //   }
    // });
  }

  async deleteClient(idClient: number): Promise<void> {
    const confirmed = await this.dialogService.showConfirm('Are you sure you want to delete this client?');
    
    if (!confirmed) return;

    try {
      await this.clientService.deleteClient(idClient).toPromise();
      this.clients = this.clients.filter(c => c.idClient !== idClient);
      this.filterClients();
      await this.dialogService.showInfo('Client deleted successfully');
    } catch (error) {
      await this.dialogService.showError('Error deleting client');
    }
  }
}
