import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/pages/login.component';
import { HomeComponent } from './shared/ui/home/home.component';
import { NotFoundComponent } from './shared/ui/not-found/not-found.component';
import { RegisterComponent } from './features/register/pages/register.component';
import { AuthGuard } from './core/guards/auth.guard';
import { DocumentsComponent } from './features/documents/pages/documents.component';
import { ForbiddenComponent } from './shared/ui/forbidden/forbidden.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    children: [
      { path: 'login', component: LoginComponent },
      // Agrega más rutas hijas aquí si es necesario
      // Ejemplo:
      { path: 'register', component: RegisterComponent},
      // { path: 'user', component: UserComponent },
      { path: 'documents', component: DocumentsComponent, canActivate: [AuthGuard] },
      // { path: 'cases', component: CasesComponent },
      // { path: 'assistant', component: AssistantComponent },
      // Ruta por defecto dentro de HomeComponent
    ]
  },
  { path: '403', component:  ForbiddenComponent}, // Ruta 403
  { path: '**', component: NotFoundComponent }, // Ruta 404
   // Ruta 403
];
