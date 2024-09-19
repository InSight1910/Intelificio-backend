import * as AuthActions from './auth.actions';
import { Injectable } from '@angular/core';

import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Router } from '@angular/router';

import { mergeMap, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';

import { jwtDecode } from 'jwt-decode';
import { User } from '../../shared/models/user.model';
import { LoginRequest } from '../../shared/models/auth.model';
import { AuthService } from '../../core/services/auth.service';

@Injectable()
export class AuthEffects {
  login$ = createEffect(() => {
    console.log(this.actions$);
    console.log(this.authService);
    return this.actions$.pipe(
      ofType(AuthActions.login),
      mergeMap((action: LoginRequest) =>
        this.authService.login(action.email, action.password).pipe(
          map((login) => {
            const user: User = this.decodeToken(login.token);
            return AuthActions.loginSuccess({ user });
          }),
          catchError((error) => {
            console.error(error);
            return of(AuthActions.loginFailure({ error: error.message }));
          })
        )
      )
    );
  });

  loginSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.loginSuccess),
        map(() => this.router.navigate(['/home']))
      ),
    { dispatch: false }
  );

  loginFailure$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.loginFailure),
        tap(() => {
          console.log('Fail');
        })
      ),
    { dispatch: false }
  );

  logout$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.logout),
        tap(() => {
          this.authService.logout();
          this.router.navigate(['/login']);
        })
      ),
    { dispatch: false }
  );

  private decodeToken(token: string): any {
    try {
      return jwtDecode(token);
    } catch (error) {
      console.error('Invalid token', error);
      return null;
    }
  }

  constructor(
    private actions$: Actions,
    private authService: AuthService,
    private router: Router
  ) {
    console.log('Test');
  }
}
