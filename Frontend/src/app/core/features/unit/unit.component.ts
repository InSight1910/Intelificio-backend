import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { UnitService } from '../../services/unit/unit.service';
import { Unit } from '../../../shared/models/unit.model';
import { ReactiveFormsModule, FormGroup, FormBuilder } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { catchError, tap, of } from 'rxjs';
import { AddModalComponent } from "./add-modal/add-modal.component";

@Component({
  selector: 'app-unit',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, AddModalComponent],
  templateUrl: './unit.component.html',
  styleUrls: ['./unit.component.css']
})
export class UnitComponent implements OnInit {
  units: Unit[] = [];
  selectedUnit: Unit | null = null;
  unitForm: FormGroup;
  isUpdating: { [key: number]: boolean } = {};
  isOpen: boolean = false;
  errors!: { message: string }[] | null;
  isSearching: boolean = false;

  @Input() communityId = '0';
  isLoading: boolean = false;

  constructor(private unitService: UnitService, private fb: FormBuilder) {
    this.unitForm = this.fb.group({
      id: [''],
      floor: [''],
      number: [''],
      surface: [''],
      user: [''],
      building: [''],
      unitType: ['']
    });
  }

  ngOnInit(): void {
    const buildingId = 1;
    this.unitService.getUnitsByBuilding(buildingId).subscribe((data) => {
      this.units = data.data;
    });
  }


  onClickEdit(unitId: number) {
    this.isUpdating[unitId] = true;
    // this.unitService
    //   .editUnit(this.unitForm.value)
    //   .subscribe(() => {
    //     this.units = this.units.filter((unit) => unit.id !== unitId);
    //     this.isUpdating[unitId] = false;
    //     this.loadUnit();
    //   });
  }

  loadUnit() {
    this.unitService
      .getUnitsByBuilding(+localStorage.getItem('unitId')!)
      .subscribe((units) => {
        this.isLoading = false;
        this.units = units.data;
      });
  }

  onCancel(): void {
    this.selectedUnit = null;
  }

  updateList(updated:boolean): void {
    if(updated) {
      this.loadUnit();
    }

  }

}

/*
import { Component, OnInit, Input } from '@angular/core';
import { UnitService } from '../../services/unit/unit.service';
import { Unit } from '../../../shared/models/unit.model';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-unit',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './unit.component.html',
  styleUrls: ['./unit.component.css']
})
export class UnitComponent implements OnInit {

  units: Unit[] = [];

  @Input() communityId = '0';
  isDeleting: { [key: number]: boolean } = {};
  isLoading: number = 0;

  constructor(private unitService: UnitService) {}

  ngOnInit(): void {
    const buildingId = 2; 
    this.unitService.getUnitsByBuilding(buildingId).subscribe((data) => {
      console.log(data);
      this.units = data.data;
    });
  }

  loadBuilding() {
    const buildingId = +localStorage.getItem('BuildingId')!;
    if (buildingId) {
      this.isLoading = true;
      this.unitService.getUnitsByBuilding(buildingId).subscribe((data) => {
        this.isLoading = false;
        this.units = data.data;
      });
    }
  }

  onClickEdit(userId: number) {
    this.isDeleting[userId] = true;
    const updateData: Partial<Unit> = {};
    this.unitService.editUnit(+this.communityId, updateData).subscribe(() => {
      this.units = this.units.filter((unit) => unit.id !== userId);
      this.isUpdating[userId] = false;
      this.loadBuilding();
    });
  }
}
*/