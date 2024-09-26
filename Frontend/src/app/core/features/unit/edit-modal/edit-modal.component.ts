import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UnitService } from '../../../services/unit/unit.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { catchError, of, tap } from 'rxjs';
import {
  UpdateUnit,
  UnitType,
  Unit,
} from '../../../../shared/models/unit.model';
import { BuildingService } from '../../../services/building/building.service';
import { Building } from '../../../../shared/models/building.model';

@Component({
  selector: 'app-edit-modal',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './edit-modal.component.html',
  styleUrl: './edit-modal.component.css',
})
export class EditModalComponent {
  @Output() editUnitEvent = new EventEmitter<boolean>();
  @Input() unitId!: string;
  editForm: FormGroup;
  isOpen: boolean = false;
  errors!: { message: string }[] | null;
  isAdding: boolean = false;
  isSuccess: boolean = false;
  canAddUnit: boolean = false;
  types: UnitType[] = [];
  buildings: Building[] = [];
  floors: number[] = [];
  unit!: Unit;

  constructor(
    private unitService: UnitService,
    private fb: FormBuilder,
    private buildingService: BuildingService
  ) {
    this.editForm = this.fb.group({
      id: [''],
      floor: [''],
      number: [''],
      surface: [''],
      user: [''],
      building: [''],
      unitTypeId: [''],
    });
  }

  onClickUpdateUnit() {
    this.isAdding = true;
    const unit: UpdateUnit = {
      id: +this.unitId,
      floor: this.editForm.get('floor')?.value,
      number: this.editForm.get('number')?.value,
      surface: this.editForm.get('surface')?.value,
      buildingId: this.editForm.get('building')?.value,
      unitTypeId: this.editForm.get('unitTypeId')?.value,
    };
    this.unitService
      .updateUnit(unit)
      .pipe(
        tap(() => {
          this.isSuccess = true;
          this.isAdding = false;
          this.editForm.disable();
          setTimeout(() => {
            this.isSuccess = false;
            this.editForm.reset();
            this.editForm.enable();
            this.floors = [];
            this.errors = null;
            this.isOpen = false;
          }, 2000);
          this.editUnitEvent.emit(true);
        }),
        catchError((error) => {
          this.canAddUnit = false;
          this.isAdding = false;
          this.errors = error.error;
          return of(error);
        })
      )
      .subscribe();
  }

  onClickCloseModal() {
    this.isOpen = false;
    this.errors = null;
    this.editForm.reset();
  }

  onClick() {
    this.unitService.getTypes().subscribe((types) => {
      this.types = types.data;
    });
    const communityId = localStorage.getItem('communityId')!;
    this.buildingService
      .getbyCommunityId(+communityId)
      .subscribe((buildings) => {
        this.buildings = buildings.data;
        this.getUnits(+this.unitId);
      });
  }

  onChangeBuilding() {
    const building = this.buildings.find(
      (x) => x.id == this.editForm.controls['building'].value
    )!;
    this.floors = Array.from(
      { length: building.floors },
      (_, index) => index + 1
    );
  }

  getUnits(id: number) {
    this.unitService.getById(id).subscribe((response) => {
      this.unit = response.data;
      this.editForm.controls['number'].setValue(this.unit.number);
      this.editForm.controls['building'].setValue(this.unit.buildingId);
      this.editForm.controls['unitTypeId'].setValue(this.unit.unitTypeId);
      this.editForm.controls['floor'].setValue(this.unit.floor);
      this.editForm.controls['surface'].setValue(this.unit.surface);
      this.onChangeBuilding();
    });
    this.isOpen = true;
  }
}
