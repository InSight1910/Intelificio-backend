import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AppState } from '../../state/auth/auth.state';
import { Inject } from '@angular/core';
import { IsAuthenticated } from '../../state/auth/auth.selectors';
import { map, take } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  // const store = Inject(Store<AppState>);
  const router = Inject(Router);

  // return store.select(IsAuthenticated).pipe(
  //   take(1),
  //   map((isAuth) => {
  //     if (isAuth) return true;
  //     router.navigate(['/login']);
  //     return false;
  //   })
  // );
  return true;
};
