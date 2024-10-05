import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule, ValidationErrors,
  ValidatorFn,
  Validators
} from "@angular/forms";
import {AppState} from "../../../../../states/intelificio.state";
import {AuthService} from "../../../../services/auth/auth.service";
import {City, Municipality, Region} from "../../../../../shared/models/location.model";
import {LocationService} from "../../../../services/location/location.service";
import {CommunityService} from "../../../../services/community/community.service";
import {tap} from "rxjs";
import {Store} from "@ngrx/store";
import {CommunityActions} from "../../../../../states/community/community.actions";
import {AdminCommunityComponent} from "../admin-community.component";

@Component({
  selector: 'app-add-community',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './add-community.component.html',
  styleUrl: './add-community.component.css'
})
export class AddCommunityComponent implements OnInit{
  isLoading: boolean = false;
  notification: boolean = false;
  IsSuccess: boolean = false;
  IsError: boolean = false;
  notificationMessage: string = "";
  loadingLocation: boolean = false;
  cities: City[] = [];
  regions: Region[] = [];
  municipalities: Municipality[] = [];
  @Output() close = new EventEmitter<void>();

  form: FormGroup;

  constructor(private store: Store<AppState>,
              private fb: FormBuilder,
              private service: CommunityService,
              private locationService: LocationService,
              private adminCommunity: AdminCommunityComponent) {
    this.form = this.fb.group({
      name: new FormControl('', Validators.required),
      rut:  new FormControl('', [Validators.required, this.rutValidator()]),
      address: new FormControl('', Validators.required),
      municipalityId: new FormControl(0, [Validators.required, Validators.min(1)]),
      cityId: new FormControl(0, [Validators.required, Validators.min(1)]),
      regionId: new FormControl(0, [Validators.required, Validators.min(1)]),
    });


  }

  ngOnInit(){
    this.cargaLocaciones();
  }

  onClose() {
    this.close.emit();
  }

  closeNotification(){
    this.notification = false;
    this.IsSuccess = false;
    this.IsError = false;
  }

  OnSubmit(){
    this.isLoading = true;
    if(this.form.valid){
      const community = {
        name: this.form.value.name,
        rut: this.form.value.rut.replace(/[.\-]/g, '').toUpperCase(),
        address: this.form.value.address,
        municipalityId: this.form.value.municipalityId
      };

      this.service.createCommunity(community).subscribe({
        next: (response) => {
          if(response.data.id != undefined){
            this.isLoading = false;
            this.notificationMessage = "Comunidad creada exitosamente."
            this.IsSuccess = true;
            this.notification = true;
            setTimeout(() => {
              this.notification = false;
              this.notificationMessage = ""
              this.IsSuccess = false;
              this.store.dispatch(
                CommunityActions.getCommunity({ id: response.data.id ?? 1 })
              );
              this.adminCommunity.getAllCommunity();
              this.close.emit();
            }, 3000);
          }
        },
        error: (error) => {
          if(error.error[0].message.length > 0){
            this.isLoading = false;
            this.notificationMessage = error.error[0].message
            this.IsError = true;
            this.notification = true;
            setTimeout(() => {
              this.notification = false;
              this.notificationMessage = ""
              this.IsError = false;
            }, 3000);
          }
        },
      });

    }
  }

  cargaLocaciones() {
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

  onChangeRegion(e: any) {
    this.locationService
      .getCitiesByRegion(e.target.value)
      .pipe(
        tap((cities) => {
          this.cities = cities.data;
          this.form.patchValue({ cityId: '0', municipalityId: '0' });
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
          this.form.patchValue({ municipalityId: '0' });
          this.form.get('municipalityId')?.enable();
        })
      )
      .subscribe();
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
