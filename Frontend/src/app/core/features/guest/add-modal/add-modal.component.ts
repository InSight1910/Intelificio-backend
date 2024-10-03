import { Component, EventEmitter, Input, Output } from '@angular/core';
import { GuestService } from '../../../services/guest/guest.service';
import { Building, CreateGuest } from '../../../../shared/models/guest.model';
import { FormGroup, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BuildingService } from '../../../services/building/building.service';
import { Unit } from '../../../../shared/models/unit.model';
import { UnitService } from '../../../services/unit/unit.service';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../states/intelificio.state';
import { selectCommunity } from '../../../../states/community/community.selectors';

@Component({
  selector: 'app-add-modal',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './add-modal.component.html',
  styleUrls: ['./add-modal.component.css']
})
export class AddModalComponent {
  @Input() buildingId: string = '0';
  @Input() unitId!: string;
  @Output() addGuestEvent = new EventEmitter<boolean>();
  isOpen = false;
  isAdding: boolean = false;
  guestForm!: FormGroup;
  buildings: Building[] = [];
  units: Unit[] = [];
  isLoading: boolean = false;

  constructor(
    private store: Store<AppState>,
    private buildingService: BuildingService,
    private unitService: UnitService,
    private guestService: GuestService, 
    private fb: FormBuilder
  ) {
    this.guestForm = this.fb.group({
      firstname: [''],
      lastname: [''],
      rut: [''],
      entrytime: [new Date()],
      plate: [''],
      building: [''],
      unit: ['']
    });
  }

  ngOnInit() {
    this.store.select(selectCommunity).subscribe((community) => {
      if (community && community.id !== undefined) {
        this.buildingService
        .getbyCommunityId(community.id)
        .subscribe((buildings) => {
          this.buildings = buildings.data;
        });
      }
    });
  }

  ngOnChanges(): void {
    if (this.buildingId !== '0') {
      this.loadUnits(+this.buildingId);
    }
  }

  loadUnits(buildingId: number) {
    this.isLoading = true;
    this.unitService.getDeptosByBuilding(buildingId).subscribe((data) => {
      this.isLoading = false;
      this.units = data.data;
    });
  }

  onClickAddGuest(): void {
    const newGuest: CreateGuest = {
      ...this.guestForm.value,
      unitId: +this.unitId
    };

    this.guestService.createGuest(newGuest).subscribe(() => {
      this.addGuestEvent.emit(true);
      this.onClickCloseModal();
    });
  }

  onChangeBuilding() {
    const selectedBuildingId = this.guestForm.get('building')?.value;
    if (selectedBuildingId) {
      this.loadUnits(+selectedBuildingId);
    }
  }

  onClickOpenModal() {
    this.isOpen = true;
  }

  onClickCloseModal() {
    this.isOpen = false;
  }
}
