import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from '../data/login.service'; // Asegúrate de que la ruta sea correcta
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Login } from '../data/login';
import { DialogService } from '../../../shared/ui/dialog/data/dialog.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [FormsModule, CommonModule],
})
export class LoginComponent {
  showPassword: boolean = false;

  user: Login = {
    email: '',
    password: '',
  };

  constructor(private loginService: LoginService, private router: Router, private dialogService: DialogService) {}

  async onSubmit(form: NgForm) {
    if (form.valid) {
      this.loginService.login(this.user.email, this.user.password).subscribe(
        async (response) => {
          await this.dialogService.showInfo('Login has been succesfull!');
          this.router.navigate(['/']);
        },
        async (error) => {
          await this.dialogService.showError('An error ocurred while trying to login.');
        }
      );
    } else {
      await this.dialogService.showError('The form is not valid');
    }
  }
}
