import { Component } from '@angular/core';
import { Register } from '../data/register';
import { Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RegisterService } from '../data/register.service';
import { DialogService } from '../../../shared/ui/dialog/data/dialog.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
  imports: [FormsModule, CommonModule],
})
export class RegisterComponent {

  showPassword: boolean = false;
  successMessage: string = '';
  errorMessage: string = '';

  user: Register = {
    name: '',
    email: '',
    password: '',
  }

  constructor(private router: Router, private registerService: RegisterService, private dialogService : DialogService) {}

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.registerService.register(this.user).subscribe({
        next: async(response) => {
          this.successMessage = response.message;
          this.errorMessage = '';
          form.resetForm();
          await this.dialogService.showInfo('Register has been succesfull!');
        },
         error: async (error: Error) => {
          this.errorMessage = error.message;
          this.successMessage = '';
          await this.dialogService.showError('An error ocurred when trying to register.');
        },
      });
    }
  }

}
