import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../states/auth/auth.state';
import { User } from '../../../../shared/models/user.model';
import { selectUser } from '../../../../states/auth/auth.selectors';
import { map, take, tap, Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { CommunityService } from '../../../services/community/community.service';
import { Community } from '../../../../shared/models/community.model';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';

import { CommunityActions } from '../../../../states/community/community.actions';
import { NavBarActions } from '../../../../states/navbar/navbar.actions';
import { Router } from '@angular/router';

@Component({
  selector: 'app-select-community',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './select-community.component.html',
  styleUrl: './select-community.component.css',
})
export class SelectCommunityComponent {
  constructor(
    private store: Store<AppState>,
    private communityService: CommunityService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.form = this.fb.group({
      address: [{ value: '', disabled: true }],
      name: [{ value: '', disabled: true }],
      adminName: [{ value: '', disabled: true }],
      buildingCount: [{ value: '', disabled: true }],
      unitCount: [{ value: '', disabled: true }],
    });
  }

  user!: Observable<User | null>;
  communities: Community[] = [];
  communitySelected!: Community;
  form: FormGroup;
  showData: boolean = false;

  ngOnInit() {
    this.store
      .select(selectUser)
      .pipe(
        take(1),
        tap((user) => {
          if (user) {
            console.log(user);
            this.communityService.getCommunitiesOfUser(user.sub!).subscribe(
              (data) => {
                this.communities = data;
              },
              (error) => {
                console.log('error');
                console.log(error);
              }
            );
          }
        })
      )
      .subscribe();
  }

  onchange(e: any) {
    const value = e.target.value;

    if (value == '|') {
      this.showData = false;
      console.log('undefined');
      return true;
    }

    this.communitySelected = this.communities.find((c) => c.id == value)!;
    this.form.patchValue(this.communitySelected!);
    this.showData = true;

    return true;
  }

  onClick() {
    console.log(this.communitySelected);
    localStorage.setItem(
      'communityId',
      JSON.stringify(this.communitySelected.id)
    );
    this.router.navigate(['/community']);
  }
}
