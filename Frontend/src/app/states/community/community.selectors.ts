import { createSelector, createFeatureSelector } from '@ngrx/store';

import { CommunityState } from './community.state';
import { selectAuthState } from '../auth/auth.selectors';

export const selectNavbarState =
  createFeatureSelector<CommunityState>('community');

export const selectCommunity = createSelector(
  selectNavbarState,
  (state) => state.community
);

export const isLoading = createSelector(
  selectNavbarState,
  (state) => state.isLoading
);

export const selectError = createSelector(
  selectNavbarState,
  (state) => state.error
);
