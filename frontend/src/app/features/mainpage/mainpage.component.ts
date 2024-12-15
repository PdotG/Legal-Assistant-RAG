import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { LoginService } from '../login/data/login.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-mainpage',
  imports: [RouterLink, CommonModule],
  templateUrl: './mainpage.component.html',
  styleUrl: './mainpage.component.css',
})
export class MainpageComponent implements OnInit {
  isLoggedIn$!: Observable<boolean>;
  constructor(private loginService: LoginService, private router: Router) {}
  ngOnInit(): void {
    this.isLoggedIn$ = this.loginService.isLoggedIn$;
  }
}
