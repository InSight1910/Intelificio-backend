import { createActionGroup, emptyProps, props } from '@ngrx/store';
import {
  Community,
  CreateCommunity,
} from '../../shared/models/community.model';

export const CommunityActions = createActionGroup({
  source: '[Community]',
  events: {
    updateCommunity: props<{ community: Community }>(),
    updateSuccess: props<{ community: Community }>(),
    updateFailure: props<{ error: string[] }>(),

    getCommunity: props<{ id: number }>(),
    getCommunitySuccess: props<{ community: Community }>(),
    getCommunityFailed: props<{ error: string[] }>(),

    createCommunity: props<{ community: CreateCommunity }>(),
    createCommunitySuccess: props<{ community: Community }>(),
    createCommunityFailed: props<{ error: string[] }>(),
  },
});
