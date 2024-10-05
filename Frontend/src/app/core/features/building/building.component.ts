import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BuildingService } from '../../services/building/building.service';
import { Building } from '../../../shared/models/building.model';
import { Store } from '@ngrx/store';
import { AppState } from '../../../states/intelificio.state';
import {
  selectCommunity,
  isLoadingCommunity,
} from '../../../states/community/community.selectors';
import { Community } from '../../../shared/models/community.model';
import { tap } from 'rxjs';
import { UnitComponent } from '../unit/unit.component';

@Component({
  selector: 'app-building',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, UnitComponent],
  templateUrl: './building.component.html',
  styleUrl: './building.component.css',
})
export class BuildingComponent implements OnInit {
  Edificios!: Building[];

  Edificio!: Building;

  buildingForm = new FormGroup({
    nombreEdificio: new FormControl('', Validators.required),
    pisosEdificio: new FormControl(0, [Validators.required, Validators.min(1)]),
    ComunidadEdificio: new FormControl('', Validators.required),
    CantidadUnidades: new FormControl(0, Validators.required),
  });

  isEdited = false;
  isDeletion = false;
  notification = false;
  isCreation = false;
  isLoading = false;
  isSubmit = false;
  postUpdateOrCreate = false;
  loading = false;
  selectedBuildingId: number = 1;
  ActivateModal = false;
  createdMessage: string = 'Edificio creado.';
  updatedMessage: string = 'Edificio actualizado.';
  community!: Community | null;
  errors: any;
  canSend: boolean = true;

  constructor(
    private service: BuildingService,
    private store: Store<AppState>
  ) {}

  async ngOnInit() {
    await this.getNumberOfBuilding();
    this.buildingForm.disable();
  }

  async getNumberOfBuilding() {
    this.isLoading = true;
    this.store
      .select(selectCommunity)
      .pipe(
        tap((c) => {
          this.community = c;
          this.service.getbyCommunityId(c?.id!).subscribe(
            (response: { data: Building[] }) => {
              this.Edificios = response.data;
              if (this.Edificios.length > 0) {
                this.selectedBuildingId = this.Edificios[0].id;
                this.detail(this.selectedBuildingId);
                this.isLoading = false;
              }
            },
            (error) => {
              this.isLoading = false;
              console.error('Error al obtener edificios', error);
            }
          );
        })
      )
      .subscribe();
  }

  private populateFields(): void {
    this.buildingForm.controls['nombreEdificio'].setValue(this.Edificio.name);
    this.buildingForm.controls['pisosEdificio'].setValue(this.Edificio.floors);
    this.buildingForm.controls['CantidadUnidades'].setValue(
      this.Edificio.units
    );
    this.buildingForm.controls['ComunidadEdificio'].setValue(
      this.Edificio.communityName
    );
  }

  private enableFields() {
    this.buildingForm.controls['nombreEdificio'].enable();
    this.buildingForm.controls['pisosEdificio'].enable();
  }

  private cleanFields() {
    this.buildingForm.reset(
      {
        nombreEdificio: '',
        pisosEdificio: 0,
        CantidadUnidades: 0,
      },
      { emitEvent: false }
    );
    this.enableFields();
  }

  detail(id: number) {
    this.selectedBuildingId = id;
    this.isEdited = false;
    this.isCreation = false;
    this.notification = false;
    const selectedBuilding = this.Edificios?.find(
      (building) => building.id === id
    );

    if (selectedBuilding) {
      this.Edificio = selectedBuilding;
      this.populateFields();
      this.buildingForm.disable();
    } else {
      console.error('Edificio no encontrado');
    }
  }

  edit() {
    this.isEdited = true;
    this.notification = false;
    this.postUpdateOrCreate = false;
    this.enableFields();
  }

  exitdit() {
    this.isEdited = false;
    this.notification = false;
    this.postUpdateOrCreate = false;
    this.populateFields();
    this.buildingForm.disable();
  }

  exitcreation() {
    this.isCreation = false;
    this.isEdited = false;
    this.notification = false;
    this.postUpdateOrCreate = false;
    this.populateFields();
    this.buildingForm.disable();
  }

  closeNotification() {
    this.notification = false;
    this.isCreation = false;
    this.isEdited = false;
    this.postUpdateOrCreate = false;
    this.isDeletion = false;
    this.postUpdateOrCreate = false;
  }

  update() {
    if (this.buildingForm.valid) {
      this.isSubmit = true;
      const updateBuilding = {
        Name: this.buildingForm.controls['nombreEdificio'].value,
        Floors: this.buildingForm.controls['pisosEdificio'].value,
        communityId: this.Edificio.communityId,
      };
      this.service.update(this.Edificio.id, updateBuilding).subscribe({
        next: (response) => {
          if (response.status === 200) {
            this.postUpdateOrCreate = true;
            setTimeout(() => {
              this.isEdited = false;
              this.postUpdateOrCreate = false;
              this.ngOnInit();
            }, 3000);
          }
          this.isSubmit = false;
        },
        error: (error) => {
          this.isSubmit = false;
          console.log('Error:', error);
        },
      });
    }
  }

  create() {
    this.isCreation = true;
    this.notification = false;
    this.postUpdateOrCreate = false;
    this.cleanFields();
  }

  saveCreate() {
    this.loading = true;
    if (this.buildingForm.valid) {
      const createBuilding = {
        Name: this.buildingForm.controls['nombreEdificio'].value,
        Floors: this.buildingForm.controls['pisosEdificio'].value,
        communityId: this.Edificio.communityId,
      };

      this.service.create(createBuilding).subscribe({
        next: (response) => {
          if (response.status === 204) {
            this.postUpdateOrCreate = true;
            setTimeout(() => {
              this.loading = false;
              this.isCreation = false;
              this.postUpdateOrCreate = false;
              this.ngOnInit();
            }, 3000);
          } else {
            this.isCreation = false;
            this.postUpdateOrCreate = false;
            this.loading = false;
            this.ngOnInit();
          }
        },
        error: (error) => {
          this.loading = false;
          this.errors = error.error;
          this.canSend = false;
          console.log('Error:', error.error);
        },
      });
    }
  }

  openmodal() {
    if (this.Edificio.units >= 1) {
      this.notification = true;
      setTimeout(() => {
        this.notification = false;
      }, 5000);
    } else {
      this.ActivateModal = true;
    }
  }

  closemodal() {
    this.ActivateModal = false;
  }

  deletebuilding() {
    if (this.Edificio.units >= 1) {
      this.notification = true;
      setTimeout(() => {
        this.notification = false;
      }, 5000);
    } else {
      this.loading = true;
      this.service.delete(this.Edificio.id).subscribe({
        next: (response) => {
          if (response.status === 200) {
            this.loading = false;
            this.ActivateModal = false;
            this.postUpdateOrCreate = true;
            this.isDeletion = true;
            setTimeout(() => {
              this.isCreation = false;
              this.isDeletion = false;
              this.postUpdateOrCreate = false;
              this.ngOnInit();
            }, 5000);
          }
        },
        error: (error) => {
          console.log('Error:', error);
        },
      });
    }
  }

  onInputChange(controlName: string): void {
    this.canSend = true;
    this.errors = null;
    const control = this.buildingForm.get(controlName);
    if (control) {
      control.markAsUntouched();
    }
  }
}
