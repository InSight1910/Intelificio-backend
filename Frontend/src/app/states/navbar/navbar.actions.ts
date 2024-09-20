import { createActionGroup, emptyProps, props } from '@ngrx/store';

export const NavBarActions = createActionGroup({
  source: '[NavBar]',
  events: {
    select: props<{ title: string }>(),
  },
});
