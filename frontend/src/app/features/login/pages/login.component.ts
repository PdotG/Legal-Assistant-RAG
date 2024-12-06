import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { NgModule } from '@angular/core';

import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [BrowserModule, FormsModule]
})
export class LoginComponent {
  showPassword: boolean = false;

  onSubmit(form: NgForm) {
    // Handle login logic here
  }
}
