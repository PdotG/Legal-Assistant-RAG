import { Component } from '@angular/core';
import { FooterComponent } from './shared/ui/footer/footer.component';
import { HeaderComponent } from './shared/ui/header/header.component';
import { RouterOutlet } from '@angular/router';
import { DialogComponent } from './shared/ui/dialog/pages/dialog.component';

@Component({
  selector: 'app-root',
  imports: [FooterComponent, HeaderComponent, RouterOutlet, DialogComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'Home';
}
