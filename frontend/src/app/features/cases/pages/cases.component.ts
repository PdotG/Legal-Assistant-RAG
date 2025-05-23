import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CaseService } from '../data/case.service';
import { MatDialog } from '@angular/material/dialog';
import { DialogService } from '../../../shared/ui/dialog/data/dialog.service';
import { LoginService } from '../../login/data/login.service';
import { Case } from '../data/case';
import { CreateCaseModalComponent } from '../../create-case-modal/pages/create-case-modal.component';
import { EditCaseModalComponent } from '../../edit-case-modal/pages/edit-case-modal.component';

@Component({
  selector: 'app-cases',
  imports: [CommonModule, FormsModule],
  templateUrl: './cases.component.html',
  styleUrl: './cases.component.css',
})
export class CasesComponent implements OnInit {
  cases: Case[] = [];
  filteredCases: Case[] = [];
  searchTerm: string = '';
  loading: boolean = false;
  currentUserId: number = 0;

  constructor(
    private caseService: CaseService,
    private authService: LoginService,
    private dialog: MatDialog,
    private dialogService: DialogService
  ) {
    const userId = this.authService.getIdUserLoggedIn();
    this.currentUserId = userId !== null ? parseInt(userId, 10) : 0;
  }

  ngOnInit(): void {
    this.loadCases();
  }

  async loadCases(): Promise<void> {
    try {
      this.loading = true;
      this.cases =
        (await this.caseService
          .getAllCasesByIdUser(this.currentUserId)
          .toPromise()) || [];
      this.filteredCases = [...this.cases];
    } catch (error) {
      await this.dialogService.showError('Error al cargar los casos');
    } finally {
      this.loading = false;
    }
  }

  filterCases(): void {
    if (!this.searchTerm) {
      this.filteredCases = [...this.cases];
      return;
    }

    this.filteredCases = this.cases.filter(
      (caso) =>
        caso.title.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        caso.description
          ?.toLowerCase()
          .includes(this.searchTerm.toLowerCase()) ||
        caso.client.name.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }

  async openCreateDialog(): Promise<void> {
    const dialogRef = this.dialog.open(CreateCaseModalComponent, {
      width: '600px',
      panelClass: ['custom-dialog-container', 'mat-dialog']
    });

    const result = await dialogRef.afterClosed().toPromise();
    if (result) {
      await this.loadCases();
    }
  }

  async editCase(idCase: number, idClient: number): Promise<void> {
    const dialogRef = this.dialog.open(EditCaseModalComponent, {
      width: '600px',
      data: { caseId: idCase, idClient: idClient },
      panelClass: ['custom-dialog-container', 'mat-dialog']
    });

    const result = await dialogRef.afterClosed().toPromise();
    if (result) {
      await this.loadCases();
    }
  }

  async deleteCase(idCase: number): Promise<void> {
    const confirmed = await this.dialogService.showConfirm(
      '¿Está seguro de que desea eliminar este caso?'
    );
    if (!confirmed) return;

    try {
      await this.caseService.deleteCase(idCase).toPromise();
      this.cases = this.cases.filter((c) => c.idCase !== idCase);
      this.filterCases();
      await this.dialogService.showInfo('Caso eliminado correctamente');
    } catch (error) {
      await this.dialogService.showError('Error al eliminar el caso');
    }
  }
}
