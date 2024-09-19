import { createSelector, createFeatureSelector } from '@ngrx/store';
import { AuthState } from '../../shared/models/user.model';

export const selectAuthState = createFeatureSelector<AuthState>('auth');

export const selectUser = createSelector(
  selectAuthState,
  (state) => state.user
);

export const selectLoading = createSelector(
  selectAuthState,
  (state) => state.loading
);

export const selectError = createSelector(
  selectAuthState,
  (state) => state.error
);

export const IsAuthenticated = createSelector(selectUser, (user) => !!user);

export const selectUserRole = createSelector(selectUser, (user) =>
  user ? user.role : []
);
