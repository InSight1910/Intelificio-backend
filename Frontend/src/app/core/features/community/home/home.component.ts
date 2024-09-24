import { ChangeDetectorRef, Component } from '@angular/core';
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
import { CommunityService } from '../../../services/community/community.service';

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
  isLoadingData: boolean = false;
  errors!: Observable<string[] | null>;

  cities: City[] = [];
  regions: Region[] = [];
  municipalities: Municipality[] = [];
  isModifying: boolean = false;

  loadingLocation: boolean = false;

  constructor(
    private store: Store<AppState>,
    private fb: FormBuilder,
    private locationService: LocationService,
    private communityService: CommunityService,
    private cdr: ChangeDetectorRef
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
    this.isLoadingData = true;
    this.loadingLocation = true;
    this.loadCommunity();
  }

  loadCommunity() {
    this.store.select(selectCommunity).subscribe((community) => {
      if (community?.adminName == null) {
        this.communityService
          .getCommunity(community!.id!)
          .subscribe(({ data }) => {
            this.isLoadingData = false;
            this.form.patchValue({
              name: data.name,
              address: data.address,
              adminName: data.adminName,
              id: community?.id,
              municipalityId: data.municipalityId,
              cityId: data.cityId,
              regionId: data.regionId,
            });
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
        this.form.disable();
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

    this.store.dispatch(
      CommunityActions.updateCommunity({ community: updateCommunity })
    );
    this.store.select(isLoading).subscribe((loading) => {
      if (!loading) {
        this.form.disable();
        this.isModifying = false;
        this.loadCommunity();
      }
    });
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

  onClickCancel() {
    this.isModifying = false;
    this.form.disable();
  }
}
