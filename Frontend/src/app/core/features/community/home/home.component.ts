import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { UsersCommunityComponent } from './users-community/users-community.component';
import {
  City,
  Municipality,
  Region,
} from '../../../../shared/models/location.model';
import { AppState } from '../../../../states/intelificio.state';
import { Store } from '@ngrx/store';
import { LocationService } from '../../../services/location/location.service';
import { CommunityService } from '../../../services/community/community.service';
import { NavBarActions } from '../../../../states/navbar/navbar.actions';
import { tap } from 'rxjs';
import { selectCommunity } from '../../../../states/community/community.selectors';
import { CommunityActions } from '../../../../states/community/community.actions';
import { Community } from '../../../../shared/models/community.model';
import { selectLoading } from '../../../../states/auth/auth.selectors';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, UsersCommunityComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeCommunityComponent {
  form: FormGroup;

  cities: City[] = [];
  regions: Region[] = [];
  municipalities: Municipality[] = [];
  isModifying: boolean = false;

  loadingLocation: boolean = false;

  constructor(
    private store: Store<AppState>,
    private fb: FormBuilder,
    private locationService: LocationService,
    private communityService: CommunityService
  ) {
    this.form = this.fb.group({
      name: [''],
      address: [''],
      adminName: [''],
      id: [0],
      municipalityId: [''],
      cityId: [''],
      regionId: [''],
    });
  }

  ngOnInit() {
    this.store.dispatch(NavBarActions.select({ title: 'Comunidad' }));
    this.loadCommunity();
  }

  loadCommunity() {
    this.store
      .select(selectCommunity)
      .pipe(
        tap((community) => {
          this.form.patchValue(community!);
          this.form.disable();
          this.loadLocation();
        })
      )
      .subscribe();
  }

  loadLocation() {
    this.loadingLocation = true;
    this.locationService.getRegions().subscribe((regions) => {
      this.regions = regions.data;
    });

    this.locationService
      .getCitiesByRegion(this.form.value.regionId)
      .subscribe((cities) => (this.cities = cities.data));

    this.locationService
      .getMunicipalitiesByCity(this.form.value.cityId)
      .subscribe((municipalities) => {
        this.municipalities = municipalities.data;
        this.loadingLocation = false;
      });
  }

  onClickEdit() {
    this.isModifying = true;
    this.form.enable();
    this.form.get('adminName')?.disable();
  }

  updateUnit() {
    const { name, address, municipalityId } = this.form.value;
    this.store.dispatch(
      CommunityActions.updateCommunity({
        community: {
          name,
          address,
          municipalityId,
          id: this.form.value.id,
        },
      })
    );

    this.store.select(selectLoading).subscribe((loading) => {
      if (!loading) {
        this.isModifying = false;
        this.form.disable();
        this.loadCommunity();
      }
    });
  }

  onChangeRegion(e: any) {
    this.locationService
      .getCitiesByRegion(e.target.value)
      .pipe(
        tap((cities) => {
          this.cities = cities.data;
          this.form.patchValue({ cityId: '|', municipalityId: '|' });
          this.form.get('municipalityId')?.disable();
        })
      )
      .subscribe();
  }

  onChangeCity(e: any) {
    this.locationService
      .getMunicipalitiesByCity(e.target.value)
      .pipe(
        tap((municipalities) => {
          this.municipalities = municipalities.data;
          this.form.patchValue({ municipalityId: '|' });
          this.form.get('municipalityId')?.enable();
        })
      )
      .subscribe();
  }

  onClickCancel() {
    this.isModifying = false;
    this.form.disable();
  }
}
