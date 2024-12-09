import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/pages/login.component';
import { HomeComponent } from './shared/ui/home/home.component';
import { RegisterComponent } from './features/register/pages/register.component';
import { AuthGuard } from './core/guards/auth.guard';
import { DocumentsComponent } from './features/documents/pages/documents.component';
import { ErrorComponent } from './shared/ui/error/error.component';
import { AboutComponent } from './features/footer-pages/about/about.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      {
        path: 'documents',
        component: DocumentsComponent,
        canActivate: [AuthGuard],
      },
      { path: 'about', component: AboutComponent },
    ],
  },
  {
    path: '403',
    component: ErrorComponent,
    data: {
      error: {
        code: 403,
        title: '403',
        message: "You don't have permission to access this resource",
      },
    },
  },
  {
    path: '**',
    component: ErrorComponent,
    data: {
      error: {
        code: 404,
        title: '404',
        message: 'The page you are looking for does not exist',
      },
    },
  },
];
