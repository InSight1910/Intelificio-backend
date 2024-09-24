import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { UnitService } from '../../services/unit/unit.service';
import { Unit } from '../../../shared/models/unit.model';
import { ReactiveFormsModule, FormGroup, FormBuilder } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AddModalComponent } from './add-modal/add-modal.component';
import { EditModalComponent } from './edit-modal/edit-modal.component';

@Component({
  selector: 'app-unit',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, AddModalComponent, EditModalComponent],
  templateUrl: './unit.component.html',
  styleUrls: ['./unit.component.css'],
})
export class UnitComponent implements OnInit {
  units: Unit[] = [];
  buildingName: string = '';
  selectedUnit: Unit | null = null;
  isUpdating: { [key: number]: boolean } = {};
  isOpen: boolean = false;
  errors!: { message: string }[] | null;
  isSearching: boolean = false;

  @Input() communityId = '0';
  isLoading: boolean = false;

  constructor(private unitService: UnitService) {
  }

  ngOnInit(): void {
    const buildingId = 2;

    this.loadUnit(buildingId);
  }

  onClickEdit(unitId: number) {
    this.isUpdating[unitId] = true;
    this.unitService
  }

  loadUnit(buildingId: number) {
    this.unitService.getUnitsByBuilding(buildingId).subscribe((data) => {
      this.units = data.data;
      this.buildingName = data.data[0].building;
    });
  }

  onCancel(): void {
    this.selectedUnit = null;
  }

  updateList(updated: boolean): void {
    if (updated) {
      this.loadUnit(2);
    }
  }

  editList(updated: boolean): void {
    if (updated) {
      this.loadUnit(2);
    }
  }
}
