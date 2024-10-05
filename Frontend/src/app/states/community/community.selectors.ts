import { createSelector, createFeatureSelector } from '@ngrx/store';

import { CommunityState } from './community.state';
import { selectAuthState } from '../auth/auth.selectors';

export const selectCommunityState =
  createFeatureSelector<CommunityState>('community');

export const selectCommunity = createSelector(
  selectCommunityState,
  (state) => state.community
);

export const isLoadingCommunity = createSelector(
  selectCommunityState,
  (state) => state.isLoading
);

export const selectCommunityError = createSelector(
  selectCommunityState,
  (state) => state.error
);
