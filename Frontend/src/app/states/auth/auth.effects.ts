import { AuthActions } from './auth.actions';
import { Injectable } from '@angular/core';

import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Router } from '@angular/router';

import { mergeMap, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';

import { jwtDecode } from 'jwt-decode';
import { User } from '../../shared/models/user.model';
import { Login, LoginRequest } from '../../shared/models/auth.model';
import { AuthService } from '../../core/services/auth/auth.service';

@Injectable()
export class AuthEffects {
  login$;
  loginSuccess$;
  loginFailure$;
  logout$;
  updateToken$;
  updateTokenSuccess$;

  private decodeToken(token: string): any {
    try {
      return jwtDecode(token);
    } catch (error) {
      console.error('Invalid token', error);
      return null;
    }
  }

  constructor(
    private router: Router,
    private actions$: Actions,
    private authService: AuthService
  ) {
    this.updateToken$ = createEffect(() => {
      return this.actions$.pipe(
        ofType(AuthActions.updateToken),
        map((action) => {
          const user: User = this.decodeToken(action.token);
          return AuthActions.updateTokenSuccess({ user });
        })
      );
    });
    this.updateTokenSuccess$ = createEffect(
      () => {
        return this.actions$.pipe(ofType(AuthActions.updateTokenSuccess));
      },
      { dispatch: false }
    );

    this.login$ = createEffect(() => {
      return this.actions$.pipe(
        ofType(AuthActions.login),
        mergeMap((action: LoginRequest) =>
          this.authService.login(action.email, action.password).pipe(
            map((login: Login) => {
              const user: User = this.decodeToken(login.data.token);
              return AuthActions.loginSuccess({ user });
            }),
            catchError((error: { error: { message: string }[] }) => {
              console.log(error);
              return of(
                AuthActions.loginFailure({
                  error: error.error.map((x) => x.message),
                })
              );
            })
          )
        )
      );
    });

    this.loginSuccess$ = createEffect(
      () =>
        this.actions$.pipe(
          ofType(AuthActions.loginSuccess),
          map(() => this.router.navigate(['/select-community']))
        ),
      { dispatch: false }
    );

    this.loginFailure$ = createEffect(
      () =>
        this.actions$.pipe(
          ofType(AuthActions.loginFailure),
          tap((e) => {
            console.log(e);
          })
        ),
      { dispatch: false }
    );

    this.logout$ = createEffect(
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
  }
}
