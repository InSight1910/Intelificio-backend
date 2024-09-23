import { Component, EventEmitter, Output } from '@angular/core';
import { UnitService } from '../../../services/unit/unit.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { catchError, of, tap } from 'rxjs';
import { CreateUnit, Unit, UnitType } from '../../../../shared/models/unit.model';
import { BuildingService } from '../../../services/building/building.service';
import { Building } from '../../../../shared/models/building.model';

@Component({
  selector: 'app-add-modal',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './add-modal.component.html',
  styleUrl: './add-modal.component.css'
})
export class AddModalComponent {
  @Output() addUnitEvent = new EventEmitter<boolean>();
  unitForm: FormGroup;
  isOpen: boolean = false;
  errors!: { message: string }[] | null;
  isAdding: boolean = false;
  isSuccess: boolean = false;
  canAddUnit: boolean = false;
  types: UnitType[] = [];
  buildings: Building[] = [];

  constructor(private unitService: UnitService, private fb: FormBuilder, private buildingService: BuildingService) {
    this.unitForm = this.fb.group({
      id: [''],
      floor: [''],
      number: [''],
      surface: [''],
      user: [''],
      building: [''],
      unitType: ['|']
    });
  }

  ngOnInit() {
    this.unitService.getTypes().subscribe((types) => {
      this.types = types.data;
    });
    const BuildingId = localStorage.getItem('communityId')!;
    this.buildingService.getbyCommunityId(+BuildingId).subscribe((buildings) => {
      this.buildings = buildings.data;
    });
  }

  onClickAddUnit() {
    this.isAdding = true;
    const unit:CreateUnit = {
      floor: this.unitForm.get('floor')?.value,
      number: this.unitForm.get('number')?.value,
      surface: 5,
      buildingId: this.unitForm.get('building')?.value,
      unitTypeId: this.unitForm.get('unitType')?.value
    };
    const BuildingId = localStorage.getItem('BuildingId')!;
    console.log(unit, BuildingId);

    this.unitService
      .createUnit(unit)
      .pipe(
        tap(() => {
          this.isSuccess = true;
          this.isAdding = false;
          setTimeout(() => {
            this.isSuccess = false;
            console.log('paso timeout');
          }, 2000);
          this.unitForm.reset();
          this.addUnitEvent.emit(true);
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
    this.unitForm.reset();
  }

}
