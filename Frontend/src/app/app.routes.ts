import { Routes } from '@angular/router';
import { LoginComponent } from './core/features/authentication/login/login.component';
import { provideEffects } from '@ngrx/effects';
import { AuthEffects } from './states/auth/auth.effects';
import { provideStore } from '@ngrx/store';
import { authReducer } from './states/auth/auth.reducer';
import { SelectCommunityComponent } from './core/features/community/select-community/select-community.component';
import { HomeCommunityComponent } from './core/features/community/home/home.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'select-community',
    component: SelectCommunityComponent,
  },
  {
    path: 'community',
    component: HomeCommunityComponent,
  },
];
