import { createReducer, on } from '@ngrx/store';
import { AuthActions } from './auth.actions';
import { AuthState } from '../../shared/models/user.model';
import { jwtDecode } from 'jwt-decode';

export const initialState: AuthState = {
  user: localStorage.getItem('token')
    ? jwtDecode(localStorage.getItem('token')!)
    : null,
  loading: false,
  error: null,
};

export const authReducer = createReducer(
  initialState,
  on(AuthActions.login, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),
  on(AuthActions.loginSuccess, (state, { user }) => ({
    ...state,
    user,
    loading: false,
    error: null,
  })),
  on(AuthActions.loginFailure, (state, { error }) => ({
    ...state,
    user: null,
    loading: false,
    error,
  })),
  on(AuthActions.logout, (state,) => ({
    ...state,
    user: null,
  }))
);
