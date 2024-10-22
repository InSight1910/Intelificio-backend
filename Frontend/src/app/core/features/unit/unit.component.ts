import { Component, Input } from '@angular/core';
import { UnitService } from '../../services/unit/unit.service';
import { Unit } from '../../../shared/models/unit.model';
import { CommonModule } from '@angular/common';
import { AddModalComponent } from './add-modal/add-modal.component';
import { EditModalComponent } from './edit-modal/edit-modal.component';
import {AddUserModalComponent} from "../community/home/users-community/add-user-modal/add-user-modal.component";

@Component({
  selector: 'app-unit',
  standalone: true,
  imports: [CommonModule, AddModalComponent, EditModalComponent, AddUserModalComponent],
  templateUrl: './unit.component.html',
  styleUrls: ['./unit.component.css'],
})

export class UnitComponent {
  @Input() buildingId: string = '0';
  @Input() buildingName: string = '';
  units: Unit[] = [];
  selectedUnit: Unit | null = null;
  isUpdating: { [key: number]: boolean } = {};
  isOpen: boolean = false;
  errors!: { message: string }[] | null;
  isSearching: boolean = false;
  isLoading: boolean = false;

  constructor(private unitService: UnitService) {}

  ngOnChanges(): void {
    if (this.buildingId != '0') {
      this.loadUnit(+this.buildingId);
    }
  }

  loadUnit(buildingId: number) {
    this.isLoading = true;
    this.unitService.getUnitsByBuilding(buildingId).subscribe((data) => {
      this.isLoading = false;
      this.units = data.data;
      this.units.forEach(unit => {
        console.log(`Unit ID: ${unit.id}, User ID: ${unit.user}`);
      });
    });
  }

  editUnit(updated: boolean): void {
    if (updated) {
      this.loadUnit(+this.buildingId);
    }
  }

  deleteUnit(unitId: number): void {
    this.unitService.deleteUnit(unitId).subscribe((data) => {
      this.loadUnit(+this.buildingId);
    });
  }

  updateList(updated: boolean): void {
    if (updated) {
      this.loadUnit(+this.buildingId);
    }
  }

  onClickEdit(unitId: number) {
    this.isUpdating[unitId] = true;
  }

  onCancel(): void {
    this.selectedUnit = null;
  }

}
