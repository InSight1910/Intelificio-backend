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
import { LocationService } from '../../../services/location/location.service';
import {
  City,
  Municipality,
  Region,
} from '../../../../shared/models/location.model';
import { UsersCommunityComponent } from "./users-community/users-community.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, UsersCommunityComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeCommunityComponent {
  form: FormGroup;
  isLoading!: Observable<boolean>;
  errors!: Observable<string[] | null>;

  cities: City[] = [];
  regions: Region[] = [];
  municipalities: Municipality[] = [];

  loadingLocation: boolean = false;

  constructor(
    private store: Store<AppState>,
    private fb: FormBuilder,
    private locationService: LocationService
  ) {
    this.form = this.fb.group({
      name: [''],
      address: [''],
      adminName: [{ value: '', disabled: true }],
      id: [0],
      municipalityId: [{ value: '', disabled: true }],
      cityId: [{ value: '', disabled: false }],
      regionId: [''],
    });
  }

  ngOnInit() {
    this.store.dispatch(NavBarActions.select({ title: 'Comunidad' }));
    var id = localStorage.getItem('communityId')!;
    this.loadingLocation = true;
    this.store.select(selectCommunity).subscribe((community) => {
      this.form.patchValue(community!);
    });

    this.locationService.getRegions().subscribe((regions) => {
      this.regions = regions.data;
    });

    console.log(this.form.value);

    this.locationService
      .getCitiesByRegion(this.form.value.regionId)
      .subscribe((cities) => (this.cities = cities.data));

    this.locationService
      .getMunicipalitiesByCity(this.form.value.cityId)
      .subscribe((municipalities) => {
        this.municipalities = municipalities.data;
        this.loadingLocation = false;
      });
    this.store.dispatch(
      CommunityActions.getCommunity({
        id: Number.parseInt(id),
      })
    );
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

  onChangeRegion() {
    this.locationService
      .getCities()
      .pipe(
        tap((cities) => {
          this.cities = cities.data;
        })
      )
      .subscribe();
  }
}
