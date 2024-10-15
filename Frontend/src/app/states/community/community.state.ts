import { Community } from '../../shared/models/community.model';
import { isLoadingCommunity } from './community.selectors';

export interface CommunityState {
  community: Community | null;
  isLoading: boolean;
  error: string[] | null;
  id: number | null;
}
