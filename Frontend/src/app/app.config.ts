import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideState, provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';

import {
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { authReducer } from './states/auth/auth.reducer';
import { AuthEffects } from './states/auth/auth.effects';
import { navbarReducer } from './states/navbar/navbar.reducer';
import { communityReducer } from './states/community/community.reducer';
import { CommunityEffects } from './states/community/community.effects';

export const appConfig: ApplicationConfig = {
  providers: [
    // provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideStore({
      auth: authReducer,
      navbar: navbarReducer,
      community: communityReducer,
    }),
    provideHttpClient(withInterceptorsFromDi()),
    provideEffects([AuthEffects, CommunityEffects]),
  ],
};
