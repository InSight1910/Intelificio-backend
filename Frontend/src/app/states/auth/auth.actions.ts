import { LoginRequest } from '../../shared/models/auth.model';
import { User } from '../../shared/models/user.model';
import { createAction, props } from '@ngrx/store';

export const login = createAction('[Auth] Login', props<LoginRequest>());

export const loginSuccess = createAction(
  '[Auth] Login Success',
  props<{ user: User }>()
);

export const loginFailure = createAction(
  '[Auth] Login Failure',
  props<{ error: string }>()
);

export const logout = createAction('[Auth] Logout');
