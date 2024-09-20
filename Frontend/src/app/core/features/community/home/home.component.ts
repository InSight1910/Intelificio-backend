import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../states/intelificio.state';
import { NavBarActions } from '../../../../states/navbar/navbar.actions';
import { Community } from '../../../../shared/models/community.model';
import { Observable, tap } from 'rxjs';
import {
  selectCommunity,
  isLoading,
  selectError,
} from '../../../../states/community/community.selectors';
import { CommonModule } from '@angular/common';
import {
  Form,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommunityActions } from '../../../../states/community/community.actions';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeCommunityComponent {
  form: FormGroup;
  isLoading!: Observable<boolean>;
  errors!: Observable<string[] | null>;
  constructor(private store: Store<AppState>, private fb: FormBuilder) {
    this.form = this.fb.group({
      name: [''],
      address: [''],
      adminName: [{ value: '', disabled: true }],
      id: [0],
      municipalityId: [''],
      cityId: [''],
      regionId: [''],
    });
  }

  ngOnInit() {
    this.store.dispatch(NavBarActions.select({ title: 'Comunidad' }));
    var id = localStorage.getItem('communityId')!;
    console.log(id);
    this.store.dispatch(
      CommunityActions.getCommunity({
        id: Number.parseInt(id),
      })
    );
    this.store.select(selectCommunity).subscribe((community) => {
      console.log(community);
      this.form.patchValue(community!);
    });
  }

  onClick() {
    const updateCommunity: Community = {
      id: this.form.value.id as number,
      name: this.form.value.name,
      address: this.form.value.address,
    };
    console.log(updateCommunity);

    this.store.dispatch(
      CommunityActions.updateCommunity({ community: updateCommunity })
    );
    this.isLoading = this.store.select(isLoading);
    this.errors = this.store.select(selectError);
  }
}
