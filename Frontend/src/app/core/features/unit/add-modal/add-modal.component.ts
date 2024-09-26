import { Component, EventEmitter, Output } from '@angular/core';
import { UnitService } from '../../../services/unit/unit.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { catchError, of, tap } from 'rxjs';
import { CreateUnit, UnitType } from '../../../../shared/models/unit.model';
import { BuildingService } from '../../../services/building/building.service';
import { Building } from '../../../../shared/models/building.model';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../states/intelificio.state';
import { selectCommunity } from '../../../../states/community/community.selectors';

@Component({
  selector: 'app-add-modal',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './add-modal.component.html',
  styleUrl: './add-modal.component.css',
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
  floors: number[] = [];

  constructor(
    private store: Store<AppState>,
    private unitService: UnitService,
    private fb: FormBuilder,
    private buildingService: BuildingService
  ) {
    this.unitForm = this.fb.group({
      id: [''],
      floor: [''],
      number: [''],
      surface: [''],
      user: [''],
      building: [''],
      unitType: [''],
    });
  }

  ngOnInit() {
    this.unitService.getTypes().subscribe((types) => {
      this.types = types.data;
    });
    this.store.select(selectCommunity).subscribe((community) => {
      this.buildingService
        .getbyCommunityId(community?.id!)
        .subscribe((buildings) => {
          this.buildings = buildings.data;
        });
    });
  }

  onClickAddUnit() {
    this.isAdding = true;
    const unit: CreateUnit = {
      floor: this.unitForm.get('floor')?.value,
      number: this.unitForm.get('number')?.value,
      surface: this.unitForm.get('surface')?.value,
      buildingId: this.unitForm.get('building')?.value,
      unitTypeId: this.unitForm.get('unitType')?.value,
    };

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

          this.unitForm.reset({
            floor: '',
            number: '',
            surface: '',
            user: '',
            building: '',
            unitType: '',
          });

          this.floors = [];
          this.errors = null;

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
    this.unitForm.reset({
      floor: '',
      number: '',
      surface: '',
      user: '',
      building: '',
      unitType: '',
      id: '',
    });
  }

  onChangeBuilding() {
    console.log(this.unitForm.get('building')?.value);
    const building = this.buildings.find(
      (x) => x.id == this.unitForm.get('building')?.value
    )!;
    console.log(building);
    this.floors = Array.from(
      { length: building.floors },
      (_, index) => index + 1
    );
  }
}
