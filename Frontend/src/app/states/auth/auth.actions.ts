import { LoginRequest } from '../../shared/models/auth.model';
import { User } from '../../shared/models/user.model';
import { createActionGroup, emptyProps, props } from '@ngrx/store';

export const AuthActions = createActionGroup({
  source: '[Auth]',
  events: {
    Login: props<LoginRequest>(),
    'Login Success': props<{ user: User }>(),
    'Login Failure': props<{ error: string[] }>(),
    Logout: emptyProps,
    'Update Token': props<{ token: string }>(),
    'Update Token Success': props<{ user: User }>(),
  },
});
