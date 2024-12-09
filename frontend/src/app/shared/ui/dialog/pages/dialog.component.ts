import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DialogService } from '../data/dialog.service';
import { DialogData } from '../data/dialog';

@Component({
  selector: 'app-dialog',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css'],
})
export class DialogComponent implements OnInit {
  dialog: DialogData | null = null;

  constructor(private dialogService: DialogService) {}

  ngOnInit() {
    this.dialogService.getDialog().subscribe(dialog => {
      this.dialog = dialog;
    });
  }

  onConfirm() {
    this.dialogService.close(true);
  }

  onCancel() {
    this.dialogService.close(false);
  }
}
