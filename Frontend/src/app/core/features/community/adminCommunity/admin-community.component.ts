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
  FormBuilder, ValidatorFn, AbstractControl, ValidationErrors,
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
import {ProfileComponent} from "../../profile/profile.component";
import {AddCommunityComponent} from "./add-community/add-community.component";

@Component({
  selector: 'app-admin-community',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ProfileComponent, AddCommunityComponent],
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
  isModalOpen: boolean = false;

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
      adminId: new FormControl(0,[Validators.required,Validators.min(1)]),
      creationDate: [''],
      municipalityId: [0],
      rut:  new FormControl('', [Validators.required, this.rutValidator()]),
      cityId: [0],
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
          this.communityForm.patchValue({ cityId: '0', municipalityId: '0' });
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
          this.communityForm.patchValue({ municipalityId: '0' });
          this.communityForm.get('municipalityId')?.enable();
        })
      )
      .subscribe();
  }

  openAddModal() {
    this.isModalOpen = true;
  }

  onClickCloseAdd() {
    this.isModalOpen = false;
  }

  closeNotification() {
    this.notificacion = false;
    this.success = false;
  }

  onClickSave() {
    const updateCommunity: Community = {
      id: this.communityForm.value.id as number,
      name: this.communityForm.value.name,
      rut: this.communityForm.value.rut.replace(/[.\-]/g, '').toUpperCase(),
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

  rutValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const rut = control.value;

      if (!rut) {
        return null;
      }

      const cleanRut = rut.replace(/[.\-]/g, '').toUpperCase();

      if (!/^[0-9]+[K0-9]$/.test(cleanRut)) {
        return { invalidRut: true };
      }

      const body = cleanRut.slice(0, -1);
      const dv = cleanRut.slice(-1).toUpperCase();

      if (body.length < 7) {
        return { invalidRut: true };
      }

      // Validar el dígito verificador
      const calculatedDV = this.calculateDV(body);
      return dv === calculatedDV ? null : { invalidRut: true };
    };
  }

  calculateDV(rut: string): string {
    let sum = 0;
    let multiplier = 2;

    for (let i = rut.length - 1; i >= 0; i--) {
      sum += +rut.charAt(i) * multiplier;
      multiplier = multiplier < 7 ? multiplier + 1 : 2;
    }

    const remainder = 11 - (sum % 11);

    if (remainder === 11) {
      return '0';
    } else if (remainder === 10) {
      return 'K';
    } else {
      return remainder.toString();
    }
  }

}
