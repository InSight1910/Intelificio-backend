import { Component, EventEmitter, Input, Output } from '@angular/core';
import { GuestService } from '../../../services/guest/guest.service';
import { Guest, UpdateGuest } from '../../../../shared/models/guest.model';
import { FormGroup, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../states/intelificio.state';
import { tap, catchError, of } from 'rxjs';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-edit-modal',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './edit-modal.component.html',
  styleUrls: ['./edit-modal.component.css'],
  providers: [DatePipe]
})
export class EditModalComponent {
  @Output() editGuestEvent = new EventEmitter<boolean>();
  @Input() guestId!: number;
  guestForm: FormGroup;
  isOpen: boolean = false;
  isAdding: boolean = false;
  isSuccess: boolean = false;
  errors: { message: string }[] | null = null;
  guest!: Guest;

  constructor(
    private guestService: GuestService,
    private fb: FormBuilder,
    private store: Store<AppState>,
    private datePipe: DatePipe
  ) {
    this.guestForm = this.fb.group({
      firstName: [''],
      lastName: [''],
      rut: [''],
      entryTime: [''],
      plate: [''],
      unit: [''],
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
        entryTime: this.formatEntryTime(this.guest.entryTime),
        plate: this.guest.plate,
        unit: this.guest.unit,
      });
    });
  }

  onClickUpdateGuest() {
    if (this.guestForm.invalid) return;

    this.isAdding = true;
    const updatedGuest: UpdateGuest = {
      id: this.guest.id,
      firstname: this.guestForm.get('firstName')?.value,
      lastname: this.guestForm.get('lastName')?.value,
      rut: this.guestForm.get('rut')?.value,
      plate: this.guestForm.get('plate')?.value,
      entryTime: this.guestForm.get('entryTime')?.value,
      unit: this.guestForm.get('unit')?.value,
    };

    this.guestService.updateGuest(updatedGuest)
      .pipe(
        tap(() => {
          this.isSuccess = true;
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

   formatEntryTime(entryTime: string): string | null {
     const date = new Date(entryTime); // Convertir string a Date
     return this.datePipe.transform(date, 'HH:MM:SS'); // Formatear la fecha
   }
}
