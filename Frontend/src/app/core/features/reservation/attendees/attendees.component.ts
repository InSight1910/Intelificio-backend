import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { ModalComponent } from '../modal/modal.component';

import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import {
  Attendee,
  AttendeeCreate,
} from '../../../../shared/models/attendee.model';
import { AttendeeService } from '../../../services/attendee/attendee.service';
import { CommonModule } from '@angular/common';
import { MessageComponent } from '../../../../shared/component/error/message.component';
import { FormatRutDirective } from '../../../../shared/directives/format-rut.directive';
import { FormatRutPipe } from '../../../../shared/pipes/format-rut.pipe';

@Component({
  selector: 'app-attendees',
  standalone: true,
  imports: [
    ModalComponent,
    ReactiveFormsModule,
    CommonModule,
    MessageComponent,
    FormatRutDirective,
    FormatRutPipe,
  ],
  templateUrl: './attendees.component.html',
  styleUrl: './attendees.component.css',
})
export class AttendeesComponent {
  @Input() reservationId: number = 0;
  @Output() onClose = new EventEmitter<number>();
  form: FormGroup;
  attendees: Attendee[] = [];
  isLoadingPost: boolean = false;
  isLoading: boolean = false;
  isLoadingDelete = signal(new Map<number, boolean>());

  errors: { message: string }[] = [];
  errorsDelete: { message: string }[] = [];

  constructor(
    private attendeeService: AttendeeService,
    private fb: FormBuilder
  ) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      rut: [
        '',
        [
          Validators.required,
          Validators.minLength(10),
          Validators.maxLength(12),
        ],
      ],
    });
  }

  ngOnInit() {
    this.loadAttendees();
  }

  closeModal() {
    this.onClose.emit(this.reservationId);
  }

  loadAttendees() {
    this.isLoading = true;
    this.attendeeService.getByReservationId(this.reservationId).subscribe({
      next: ({ data }) => {
        this.attendees = data;
        this.isLoading = false;
      },
      error: (error) => {
        if (error.status === 404) {
          this.isLoading = false;
        }
      },
    });
  }

  onSubmit(event: Event) {
    event.preventDefault();
    console.log(this.form.get('rut')?.errors);
    if (this.form.valid) {
      this.isLoadingPost = true;
      this.form.disable();
      var attendee: AttendeeCreate = {
        ...this.form.value,
        rut: this.form.value.rut.replace(/[.-]/g, ''),
        reservationId: this.reservationId,
      };
      this.attendeeService.createAttendee(attendee).subscribe({
        next: ({ data }) => {
          console.log(data);
          this.attendees.push(data);
          this.form.reset();
          this.form.enable();
          this.isLoadingPost = false;
        },
        error: ({ error }) => {
          this.form.enable();
          this.errors = error;
          this.isLoadingPost = false;
          this.form.reset();
        },
      });
    }
  }

  onDelete(attendeeId: number) {
    const isLoadingDelete = this.isLoadingDelete();
    if (isLoadingDelete.get(attendeeId)) return;
    isLoadingDelete.set(attendeeId, true);
    this.attendeeService.deleteAttendee(attendeeId).subscribe({
      next: () => {
        isLoadingDelete.set(attendeeId, false);
        this.attendees = this.attendees.filter((a) => a.id !== attendeeId);
      },
      error: ({ error }) => {
        isLoadingDelete.set(attendeeId, false);
        this.errorsDelete = error;
      },
    });
  }
}
