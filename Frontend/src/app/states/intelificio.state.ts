import { AuthState } from '../shared/models/user.model';
import { CommunityState } from './community/community.state';
import { NavBarState } from './navbar/navbar.state';

export interface AppState {
  auth: AuthState;
  navbar: NavBarState;
  community: CommunityState;
}
