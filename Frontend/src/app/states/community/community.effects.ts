import { Injectable } from '@angular/core';

import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Router } from '@angular/router';

import { catchError, map, merge, mergeMap, of } from 'rxjs';

import { CommunityService } from '../../core/services/community/community.service';
import { CommunityActions } from './community.actions';
import { Community } from '../../shared/models/community.model';

@Injectable()
export class CommunityEffects {
  update$;
  updateSuccess$;
  updateFailure$;

  getCommunity$;
  getCommunitySuccess$;
  getCommunityFailed$;

  constructor(
    private router: Router,
    private actions$: Actions,
    private communityService: CommunityService
  ) {
    this.getCommunity$ = createEffect(() =>
      this.actions$.pipe(
        ofType(CommunityActions.getCommunity),
        mergeMap(({ id }) => {
          console.log(id);
          return this.communityService.getCommunity(id).pipe(
            map((data: { data: Community }) => {
              localStorage.setItem('community', JSON.stringify(data.data));
              return CommunityActions.getCommunitySuccess({
                community: data.data,
              });
            }),
            catchError((error: { error: { message: string }[] }) => {
              return of(
                CommunityActions.getCommunityFailed({
                  error: error.error.map((x) => x.message),
                })
              );
            })
          );
        })
      )
    );

    this.getCommunitySuccess$ = createEffect(
      () =>
        this.actions$.pipe(
          ofType(CommunityActions.getCommunitySuccess),
          mergeMap(() => {
            return of(this.router.navigate(['/community']));
          })
        ),
      {
        dispatch: false,
      }
    );
    this.getCommunityFailed$ = createEffect(
      () =>
        this.actions$.pipe(
          ofType(CommunityActions.getCommunityFailed),
          mergeMap(() => {
            return of(this.router.navigate(['/community']));
          })
        ),
      {
        dispatch: false,
      }
    );

    this.update$ = createEffect(() => {
      return this.actions$.pipe(
        ofType(CommunityActions.updateCommunity),
        mergeMap(({ community }) => {
          console.log(community);
          return this.communityService.updateCommunity(community).pipe(
            map(() => {
              localStorage.setItem('community', JSON.stringify(community));
              return CommunityActions.updateSuccess({ community });
            }),
            catchError((error: { error: { message: string }[] }) => {
              console.log(error);
              return of(
                CommunityActions.updateFailure({
                  error: error.error.map((x) => x.message),
                })
              );
            })
          );
        })
      );
    });

    this.updateSuccess$ = createEffect(
      () => {
        return this.actions$.pipe(
          ofType(CommunityActions.updateSuccess),
          mergeMap(() => {
            return of(this.router.navigate(['/community']));
          })
        );
      },
      { dispatch: false }
    );

    this.updateFailure$ = createEffect(
      () => {
        return this.actions$.pipe(
          ofType(CommunityActions.updateFailure),
          mergeMap(() => {
            return of(this.router.navigate(['/community']));
          })
        );
      },
      { dispatch: false }
    );
  }
}
