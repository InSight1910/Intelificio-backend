import { Component, OnInit } from '@angular/core';
import {
  Community,
  AllCommunity,
  UserAdmin,
} from '../../../../shared/models/community.model';
import { select, Store } from '@ngrx/store';
import { AppState } from '../../../../states/auth/auth.state';
import { CommunityService } from '../../../services/community/community.service';
import { CommonModule } from '@angular/common';
import {
  FormControl,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  FormBuilder,
} from '@angular/forms';
import { LocationService } from '../../../services/location/location.service';
import { CommunityActions } from '../../../../states/community/community.actions';
import {
  City,
  Municipality,
  Region,
} from '../../../../shared/models/location.model';
import { Observable, tap } from 'rxjs';
import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-admin-community',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './admin-community.component.html',
  styleUrl: './admin-community.component.css',
})
export class AdminCommunityComponent implements OnInit {
  communities!: AllCommunity[];
  cities: City[] = [];
  regions: Region[] = [];
  municipalities: Municipality[] = [];
  isLoading = false;
  loading = false;
  updating = false;
  notificacion = false;
  success = false;
  adminUsers!: UserAdmin[];

  ActivateModal = false;
  loadingLocation: boolean = false;
  communityForm: FormGroup;

  constructor(
    private service: CommunityService,
    private authservice: AuthService,
    private locationService: LocationService,
    private fb: FormBuilder,
    private store: Store<AppState>
  ) {
    this.communityForm = this.fb.group({
      id: [''],
      name: [''],
      address: [''],
      adminName: [''],
      adminId: [0],
      creationDate: [''],
      municipality: [''],
      municipalityId: [0],
      city: [''],
      cityId: [0],
      region: [''],
      regionId: [0],
    });
  }

  ngOnInit(): void {
    this.getAllCommunity();
  }

  getAllCommunity() {
    this.isLoading = true;
    this.service.getAllCommunity().subscribe(
      (response: { data: AllCommunity[] }) => {
        this.communities = response.data;
        this.isLoading = false;
      },
      (error) => {
        console.error('Error al obtener comunidades', error);
      }
    );
  }

  getAllAdmins() {
    this.authservice.getAllUserAdmin().subscribe(
      (response: { data: UserAdmin[] }) => {
        this.adminUsers = response.data;
      },
      (error) => {
        console.error('Error al obtener comunidades', error);
      }
    );
  }

  closemodal() {
    this.ActivateModal = false;
    this.success = false;
    this.notificacion = false;
  }

  edit(id: number) {
    this.loadingLocation = true;
    this.communityForm.disable();
    this.getAllAdmins();

    const communitySelected = this.communities.find(
      (AllCommunity) => AllCommunity.id === id
    );

    if (communitySelected) {
      this.communityForm.patchValue(communitySelected);
    }

    this.communityForm.enable();

    this.locationService.getRegions().subscribe((regions) => {
      this.regions = regions.data;
    });

    this.locationService
      .getCitiesByRegion(+this.communityForm.value.regionId)
      .subscribe((cities) => (this.cities = cities.data));

    this.locationService
      .getMunicipalitiesByCity(+this.communityForm.value.cityId)
      .subscribe((municipalities) => {
        this.municipalities = municipalities.data;
        this.loadingLocation = false;
      });

    this.ActivateModal = true;
  }

  onChangeRegion(e: any) {
    this.locationService
      .getCitiesByRegion(e.target.value)
      .pipe(
        tap((cities: { data: City[] }) => {
          this.cities = cities.data;
          this.communityForm.patchValue({ cityId: '|', municipalityId: '|' });
          this.communityForm.get('municipalityId')?.disable();
        })
      )
      .subscribe();
  }

  onChangeCity(e: any) {
    this.locationService
      .getMunicipalitiesByCity(e.target.value)
      .pipe(
        tap((municipalities: { data: Municipality[] }) => {
          this.municipalities = municipalities.data;
          this.communityForm.patchValue({ municipalityId: '|' });
          this.communityForm.get('municipalityId')?.enable();
        })
      )
      .subscribe();
  }

  closeNotification() {
    this.notificacion = false;
    this.success = false;
  }

  onClickSave() {
    const updateCommunity: Community = {
      id: this.communityForm.value.id as number,
      name: this.communityForm.value.name,
      address: this.communityForm.value.address,
      municipalityId: this.communityForm.value.municipalityId,
      cityId: this.communityForm.value.cityId,
      regionId: this.communityForm.value.regionId,
      adminId: this.communityForm.value.adminId,
    };
    this.updating = true;
    this.service.updateCommunity(updateCommunity).subscribe({
      next: (response) => {
        if (response.status === 200) {
          this.updating = false;
          this.communityForm.disable();
          this.success = true;
          this.notificacion = true;
          setTimeout(() => {
            this.getAllCommunity();
            this.success = false;
            this.notificacion = false;
            this.ActivateModal = false;
            this.communityForm.enable();
            this.store.dispatch(
              CommunityActions.getCommunity({ id: this.communityForm.value.id })
            );
          }, 3000);
        }
      },
      error: (error) => {
        console.log('Error:', error);
      },
    });
  }
}
