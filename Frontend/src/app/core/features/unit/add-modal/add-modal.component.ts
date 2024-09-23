import { Component, EventEmitter, Output } from '@angular/core';
import { UnitService } from '../../../services/unit/unit.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { catchError, of, tap } from 'rxjs';
import { Unit, UnitType } from '../../../../shared/models/unit.model';

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

  constructor(private unitService: UnitService, private fb: FormBuilder) {
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
  }

  onClickAddUnit() {
    this.isAdding = true;
    const unit:Unit = {
      floor: this.unitForm.get('floor')?.value,
      number: this.unitForm.get('number')?.value,
      surface: this.unitForm.get('surface')?.value,
      user: this.unitForm.get('user')?.value,
      building: this.unitForm.get('building')?.value,
      unitType: this.unitForm.get('unitType')?.value
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
