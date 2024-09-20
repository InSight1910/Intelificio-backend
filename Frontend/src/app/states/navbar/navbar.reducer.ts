import { createReducer, on } from '@ngrx/store';

import { AuthState } from '../../shared/models/user.model';
import { NavBarState } from './navbar.state';
import { NavBarActions } from './navbar.actions';

export const initialState: NavBarState = {
  title: 'Intelificio',
};

export const navbarReducer = createReducer(
  initialState,
  on(NavBarActions.select, (state, { title }) => ({ ...state, title }))
);
