import { Injectable } from '@angular/core';

import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Router } from '@angular/router';

import { catchError, map, merge, mergeMap, of, tap } from 'rxjs';

import { CommunityService } from '../../core/services/community/community.service';
import { CommunityActions } from './community.actions';
import { Community } from '../../shared/models/community.model';

@Injectable()
export class CommunityEffects {
  create$;
  createSuccess$;
  createFailure$;

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
          return this.communityService.getCommunity(id).pipe(
            map(({ data }) => {
              return CommunityActions.getCommunitySuccess({
                community: data,
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
          mergeMap(({ community }) => {
            localStorage.setItem('community', JSON.stringify(community));
            return of();
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
            return of();
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
          return this.communityService.updateCommunity(community).pipe(
            map(({ body }) => {
              return CommunityActions.updateSuccess({ community: body?.data! });
            }),
            catchError((error: { error: { message: string }[] }) => {
              console.error(error);
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
          mergeMap(({ community }) => {
            console.log(community);
            const communityLocal = JSON.parse(
              localStorage.getItem('community')!
            );
            console.log(communityLocal);
            const communityUpdated = {
              ...community,
              adminName: communityLocal.adminName,
            };
            console.log(communityUpdated);
            localStorage.setItem('community', JSON.stringify(communityUpdated));
            return of();
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

    this.create$ = createEffect(() => {
      return this.actions$.pipe(
        ofType(CommunityActions.createCommunity),
        mergeMap(({ community }) => {
          return this.communityService.createCommunity(community).pipe(
            map(({ data }) => {
              return CommunityActions.createCommunitySuccess({
                community: data,
              });
            }),
            catchError((error: { error: { message: string }[] }) => {
              return of(
                CommunityActions.createCommunityFailed({
                  error: error.error.map((x) => x.message),
                })
              );
            })
          );
        })
      );
    });
    this.createSuccess$ = createEffect(
      () => {
        return this.actions$.pipe(
          ofType(CommunityActions.createCommunitySuccess),
          mergeMap(({ community }) => {
            localStorage.setItem('community', JSON.stringify(community));
            return of(this.router.navigate(['/community']));
          })
        );
      },
      { dispatch: false }
    );
    this.createFailure$ = createEffect(
      () => {
        return this.actions$.pipe(
          ofType(CommunityActions.createCommunityFailed)
        );
      },
      { dispatch: false }
    );
  }
}
