import { Component, EventEmitter, Input, Output } from '@angular/core';
import { GuestService } from '../../../services/guest/guest.service';
import { Guest, UpdateGuest } from '../../../../shared/models/guest.model';
import {
  FormGroup,
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../states/intelificio.state';
import { tap, catchError, of, Observable, map } from 'rxjs';
import { CommonModule, DatePipe } from '@angular/common';
import { BuildingService } from '../../../services/building/building.service';
import { selectCommunity } from '../../../../states/community/community.selectors';
import { Building } from '../../../../shared/models/building.model';
import { Unit } from '../../../../shared/models/unit.model';
import { UnitService } from '../../../services/unit/unit.service';

@Component({
  selector: 'app-edit-modal',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './edit-modal.component.html',
  styleUrls: ['./edit-modal.component.css'],
  providers: [DatePipe],
})
export class EditModalComponent {
  @Output() editGuestEvent = new EventEmitter<boolean>();
  @Input() guestId!: number;
  guestForm: FormGroup;
  isOpen: boolean = false;
  isAdding: boolean = false;
  errors: { message: string }[] | null = null;
  guest!: Guest;
  buildings: Observable<Building[]> = new Observable<Building[]>();
  units: Observable<Unit[]> = new Observable<Unit[]>();

  constructor(
    private guestService: GuestService,
    private buildingService: BuildingService,
    private fb: FormBuilder,
    private store: Store<AppState>,
    private datePipe: DatePipe,
    private unitService: UnitService
  ) {
    this.guestForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      rut: ['', Validators.required],
      entryTime: ['', Validators.required],
      plate: [''],
      buildingId: ['', Validators.required],
      unitId: ['', Validators.required],
    });
  }

  onClick() {
    this.isOpen = true;
    this.store.select('community').subscribe((community) => {
      if (community && community.id !== undefined) {
        this.getGuest(this.guestId);
      }
    });
  }

  getGuest(guestId: number) {
    this.guestService.getGuestById(guestId).subscribe((response) => {
      this.guest = response.data;
      this.guestForm.patchValue({
        firstName: this.guest.firstName,
        lastName: this.guest.lastName,
        rut: this.guest.rut,
        entryTime: this.guest.entryTime.substring(11, 19),
        plate: this.guest.plate,
        buildingId: this.guest.buildingId,
        unitId: this.guest.unitId,
      });
      this.getBuildings();
      this.getUnitsByBuilding();
    });
  }

  onClickUpdateGuest() {
    if (this.guestForm.invalid) return;

    this.isAdding = true;
    const updatedGuest: UpdateGuest = {
      id: this.guestId,
      firstName: this.guestForm.get('firstName')?.value,
      lastName: this.guestForm.get('lastName')?.value,
      rut: this.guestForm.get('rut')?.value,
      plate: this.guestForm.get('plate')?.value,
      entryTime: this.guestForm.get('entryTime')?.value as Date,
      unitId: this.guestForm.get('unitId')?.value,
    };

    this.guestService
      .updateGuest(updatedGuest)
      .pipe(
        tap(() => {
          this.isAdding = false;
          this.guestForm.disable();
          setTimeout(() => {
            this.closeModal();
            this.editGuestEvent.emit(true);
          }, 2000);
        }),
        catchError((error) => {
          this.errors = error.error;
          this.isAdding = false;
          return of(error);
        })
      )
      .subscribe();
  }

  closeModal() {
    this.isOpen = false;
    this.errors = null;
    this.guestForm.reset();
    this.guestForm.enable();
  }

  onClickCloseModal() {
    this.closeModal();
  }

  getBuildings() {
    this.store.select(selectCommunity).subscribe((community) => {
      this.buildings = this.buildingService
        .getbyCommunityId(community?.id!)
        .pipe(map((response) => response.data));
    });
  }

  getUnitsByBuilding() {
    const buildingId = this.guestForm.get('buildingId')?.value;
    this.units = this.unitService
      .getDeptosByBuilding(buildingId)
      .pipe(map((response) => response.data));
  }

  formatEntryTime(entryTime: string): string | null {
    const date = new Date(entryTime);
    return this.datePipe.transform(date, 'HH:MM:SS'); // Formatear la fecha
  }
}
