import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UnitService } from '../../../services/unit/unit.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { catchError, of, tap } from 'rxjs';
import {
  UpdateUnit,
  UnitType,
  Unit
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
      unitType: [''],
    });
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
      });
      this.getUnits(+this.unitId);
    }

  onClickUpdateUnit() {
    this.isAdding = true;
    const unit: UpdateUnit = {
      id: +this.unitId,
      floor: this.editForm.get('floor')?.value,
      number: this.editForm.get('number')?.value,
      surface: this.editForm.get('surface')?.value,
      buildingId: this.editForm.get('building')?.value,
      unitTypeId: this.editForm.get('unitType')?.value,
    };
    console.log(unit);
    this.unitService
      .updateUnit(unit)
      .pipe(
        tap(() => {
          this.isSuccess = true;
          this.isAdding = false;
          setTimeout(() => {
            this.isSuccess = false;
            console.log('paso timeout');
          }, 2000);

          this.editForm.reset({
            floor: '',
            number: '',
            surface: '',
            user: '',
            building: '',
            unitType: '',
          });

          this.floors = [];
          this.errors = null;

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

  onClickOpenModal() {
    this.isOpen = true;
  }

  onClickCloseModal() {
    this.isOpen = false;
    this.errors = null;
    this.editForm.reset();
  }

  onChangeBuilding() {
    const building = this.buildings.find(
      (x) => x.id == this.editForm.get('building')?.value
    )!;
    this.floors = Array.from(
      { length: building.floors },
      (_, index) => index + 1
    );
  }

  getUnits(id: number) {
    this.unitService.getById(id).subscribe((response) => {
      this.unit = response.data;
      console.log(response.data);
      console.log(this.unit);
      this.editForm.patchValue({
        id: this.unit.id,
        floor: this.unit.floor,
        number: this.unit.number,
        surface: this.unit.surface,
        user: this.unit.user,
        building: this.unit.building,
        unitType: this.unit.unitType,
      });
    }  );  
}

}
