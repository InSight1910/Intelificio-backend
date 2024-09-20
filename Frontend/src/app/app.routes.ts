import { Routes } from '@angular/router';
import { LoginComponent } from './core/features/authentication/login/login.component';
import { BuildingComponent } from './core/features/building/building.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'Edificios',
    component: BuildingComponent
  }
  // {
  //   path: '**',
  //   redirectTo: 'login',
  // },
];
