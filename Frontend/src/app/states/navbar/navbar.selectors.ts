import { createSelector, createFeatureSelector } from '@ngrx/store';

import { NavBarState } from './navbar.state';

export const selectNavbarState = createFeatureSelector<NavBarState>('navbar');

export const selectTitle = createSelector(
  selectNavbarState,
  (state) => state.title
);
