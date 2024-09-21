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
import { UsersCommunityComponent } from './users-community/users-community.component';

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
  isModifying: boolean = false;

  loadingLocation: boolean = false;

  constructor(
    private store: Store<AppState>,
    private fb: FormBuilder,
    private locationService: LocationService
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
    var id = localStorage.getItem('communityId')!;
    console.log(id);
    this.loadingLocation = true;
    this.store.select(selectCommunity).subscribe((community) => {
      this.form.patchValue(community!);
      this.form.disable();
      if (community != null) {
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
    });
  }

  onClickSave() {
    const updateCommunity: Community = {
      id: this.form.value.id as number,
      name: this.form.value.name,
      address: this.form.value.address,
      municipalityId: this.form.value.municipalityId,
      cityId: this.form.value.cityId,
      regionId: this.form.value.regionId,
    };
    console.log(updateCommunity);

    this.store.dispatch(
      CommunityActions.updateCommunity({ community: updateCommunity })
    );
    this.isLoading = this.store.select(isLoading).pipe(
      tap((loading) => {
        if (!loading) {
          this.form.disable();
          this.isModifying = false;
        }
      })
    );
    this.errors = this.store.select(selectError);
  }
  onClickEdit() {
    this.isModifying = true;
    this.form.enable();
    this.form.get('adminName')?.disable();
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
}
