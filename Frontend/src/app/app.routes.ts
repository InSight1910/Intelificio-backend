import { Routes } from '@angular/router';
import { LoginComponent } from './core/features/authentication/login/login.component';
import { provideEffects } from '@ngrx/effects';
import { AuthEffects } from './states/auth/auth.effects';
import { provideStore } from '@ngrx/store';
import { authReducer } from './states/auth/auth.reducer';
import { SelectCommunityComponent } from './core/features/community/select-community/select-community.component';
import { HomeCommunityComponent } from './core/features/community/home/home.component';
import { UnitComponent } from './core/features/unit/unit.component';

import { BuildingComponent } from './core/features/building/building.component';
import { SingupComponent } from './core/features/authentication/signup/signup.component';
import { ChangePasswordOneComponent } from './core/features/authentication/change-password-one/change-password-one.component';
import { ChangePasswordTwoComponent } from './core/features/authentication/change-password-two/change-password-two.component';
import { AdminCommunityComponent } from './core/features/community/adminCommunity/admin-community.component';
import { HomeSpaceComponent } from './core/features/common-space/home/home.component';
import { ManageComponent } from './core/features/common-space/manage/manage.component';
import { NotificationComponent } from './core/features/notification/notification.component';
import {ContactListComponent} from "./core/features/contact-list/contact-list.component";

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'Edificios',
    component: BuildingComponent,
  },
  {
    path: 'RegistroUsuario',
    component: SingupComponent,
  },
  {
    path: 'forgot-password',
    component: ChangePasswordOneComponent,
  },
  {
    path: 'change-password',
    component: ChangePasswordTwoComponent,
  },
  // {
  //   path: '**',
  //   redirectTo: 'login',
  // },
  {
    path: 'select-community',
    component: SelectCommunityComponent,
  },
  {
    path: 'Comunidad',
    component: HomeCommunityComponent,
  },
  {
    path: 'unit',
    component: UnitComponent,
  },
  {
    path: 'Contactos',
    component: ContactListComponent,
  },
  {
    path: 'AdminComunidades',
    component: AdminCommunityComponent,
  },
  {
    path: 'AdminEspaciosComunes',
    component: ManageComponent,
  },
  {
    path: 'EspacioComun',
    component: HomeSpaceComponent,
  },
  {
    path: 'Comunicado',
    component: NotificationComponent,
  },
];
