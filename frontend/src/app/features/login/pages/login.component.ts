import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { Login } from '../data/login';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [ FormsModule]
})
export class LoginComponent {

  showPassword: boolean = false;

  user: Login = {
    email: '',
    password: ''
  };


  onSubmit(form: NgForm) {
    if (form.valid) {
      // Aquí puedes implementar la lógica de login,
      // por ejemplo, llamar a un servicio que valide al usuario.
      console.log('Datos del formulario:', form.value);
      console.log('Email:', this.user.email);
      console.log('Password:', this.user.password);

      // Ejemplo de lógica (ficticia)
      // this.authService.login(this.email, this.password)
      //   .subscribe(response => {
      //     // Manejar respuesta de login
      //   });

    } else {
      console.log('El formulario no es válido.');
    }
  }
}
