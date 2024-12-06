import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from '../data/login.service'; // Asegúrate de que la ruta sea correcta
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Login } from '../data/login';

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

  constructor(private loginService: LoginService, private router: Router) {}

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.loginService.login(this.user.email, this.user.password).subscribe(
        (response) => {
          console.log('Login exitoso:', response);
          this.router.navigate(['/']);
        },
        (error) => {
          console.error('Error en el login:', error);
        }
      );
    } else {
      console.log('El formulario no es válido.');
    }
  }
}
