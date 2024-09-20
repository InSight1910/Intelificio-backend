import { createReducer, on } from '@ngrx/store';

import { CommunityActions } from './community.actions';

import { CommunityState } from './community.state';
import { Community } from '../../shared/models/community.model';

export const initialState: CommunityState = {
  community: localStorage.getItem('community')
    ? JSON.parse(localStorage.getItem('community')!)
    : null,
  isLoading: false,
  id: null,
  error: null,
};

export const communityReducer = createReducer(
  initialState,
  on(CommunityActions.updateCommunity, (state, { community }) => ({
    ...state,
    community,
    isLoading: true,
    error: null,
  })),
  on(CommunityActions.updateSuccess, (state, { community }) => ({
    ...state,
    isLoading: false,
    community,
    error: null,
  })),
  on(CommunityActions.updateFailure, (state, { error }) => ({
    ...state,
    community: null,
    error,
    isLoading: false,
  })),
  on(CommunityActions.getCommunity, (state, { id }) => ({
    ...state,
    id,
    isLoading: true,
    error: null,
  })),
  on(CommunityActions.getCommunitySuccess, (state, { community }) => ({
    ...state,
    community,
    isLoading: false,
  })),
  on(CommunityActions.getCommunityFailed, (state, { error }) => ({
    ...state,
    error,
    community: null,
    isLoading: false,
  }))
);
