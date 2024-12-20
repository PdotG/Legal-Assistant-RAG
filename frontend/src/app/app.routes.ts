import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/pages/login.component';
import { HomeComponent } from './shared/ui/home/home.component';
import { RegisterComponent } from './features/register/pages/register.component';
import { AuthGuard } from './core/guards/auth.guard';
import { DocumentsComponent } from './features/documents/pages/documents.component';
import { UserProfileComponent } from './features/user-profile/pages/user-profile.component';
import { ErrorComponent } from './shared/ui/error/error.component';
import { AboutComponent } from './features/footer-pages/about/about.component';
import { ContactComponent } from './features/footer-pages/contact/contact.component';
import { TermsComponent } from './features/footer-pages/terms/terms.component';
import { MainpageComponent } from './features/mainpage/mainpage.component';
import { ClientsComponent } from './features/clients/pages/clients.component';
import { GetstartedComponent } from './features/getstarted/getstarted.component';
import { CasesComponent } from './features/cases/pages/cases.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    children: [
      { path: '', component: MainpageComponent },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      {
        path: 'documents',
        component: DocumentsComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'profile',
        component: UserProfileComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'clients',
        component: ClientsComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'cases',
        component: CasesComponent,
        canActivate: [AuthGuard],
      },
      { path: 'about', component: AboutComponent },
      { path: 'contact', component: ContactComponent },
      { path: 'terms', component: TermsComponent },
      { path: 'getstarted', component: GetstartedComponent },
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
