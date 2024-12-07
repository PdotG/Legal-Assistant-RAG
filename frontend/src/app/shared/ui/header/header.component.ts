import {Component, OnInit} from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { Observable } from 'rxjs';
import { LoginService } from '../../../features/login/data/login.service';
import { CommonModule } from '@angular/common';

/**
 * @title Menu positioning
 */
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  imports: [RouterLink, CommonModule],
})
export class HeaderComponent implements OnInit {
  isLoggedIn$!: Observable<boolean>;

  constructor(private loginService: LoginService, private router: Router) {}

  ngOnInit(): void {
    // Suscribirse al observable de estado de login
    this.isLoggedIn$ = this.loginService.isLoggedIn$;
  }

  // Método para cerrar sesión
  logout(): void {
    this.loginService.logout();
    // Opcional: Navegar a la página de inicio o login después de cerrar sesión
    this.router.navigate(['/login']);
  }
}
