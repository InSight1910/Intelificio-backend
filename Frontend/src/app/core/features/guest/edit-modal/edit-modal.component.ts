import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { GuestService } from '../../../services/guest/guest.service';
import { UpdateGuest } from '../../../../shared/models/guest.model';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommunityService } from '../../../services/community/community.service';

@Component({
  selector: 'app-edit-modal',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './edit-modal.component.html',
  styleUrls: ['./edit-modal.component.css']
})
export class EditModalComponent implements OnInit {
  @Input() guestId!: number;
  @Output() editGuestEvent = new EventEmitter<boolean>();
  isOpen = false;
  guestForm!: FormGroup;

  constructor(
    private guestService: GuestService, 
    private fb: FormBuilder,
    private communityService: CommunityService,
  ) {
    this.guestForm = this.fb.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      rut: ['', Validators.required],
      entrytime: [new Date(), Validators.required],
      plate: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.guestService.getGuestById(this.guestId).subscribe((guest) => {
      this.guestForm.patchValue(guest);
    });
  }

  onClickOpenModal() {
    this.isOpen = true;
  }

  onClickCloseModal() {
    this.isOpen = false;
  }

  onClickUpdateGuest(): void {
    const updatedGuest: UpdateGuest = {
      id: this.guestId,
      ...this.guestForm.value,
      entrytime: new Date() // Actualiza la fecha si es necesario
    };

    this.guestService.updateGuest(this.guestId, updatedGuest).subscribe(() => {
      this.editGuestEvent.emit(true);
      this.onClickCloseModal();
    });
  }
}
