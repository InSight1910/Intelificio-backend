import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { Community } from '../../../../shared/models/community.model';
import { AppState } from '../../../../states/intelificio.state';
import { Store } from '@ngrx/store';
import { selectCommunity } from '../../../../states/community/community.selectors';
import { Observable } from 'rxjs';
import { ReservationService } from '../../../services/reservation/reservation.service';
import { User } from '../../../../shared/models/user.model';
import { selectUser } from '../../../../states/auth/auth.selectors';
import {
  MyReservation,
  ReservationStatus,
} from '../../../../shared/models/reservation.model';
import { ModalComponent } from '../modal/modal.component';
import { AttendeesComponent } from '../attendees/attendees.component';

@Component({
  selector: 'app-my-reservations',
  standalone: true,
  imports: [CommonModule, ModalComponent, AttendeesComponent],
  templateUrl: './my-reservations.component.html',
  styleUrl: './my-reservations.component.css',
})
export class MyReservationsComponent {
  community!: Observable<Community | null>;
  user!: Observable<User | null>;
  reservations: MyReservation[] = [];
  ReservationStatus = ReservationStatus;
  isOpenAddAttendees = signal(new Map<number, boolean>());

  constructor(
    private store: Store<AppState>,
    private reservationService: ReservationService
  ) {}

  ngOnInit() {
    this.community = this.store.select(selectCommunity);
    this.user = this.store.select(selectUser);
    this.loadReservations();
  }

  loadReservations() {
    this.user.subscribe((user) => {
      if (user) {
        this.reservationService
          .getReservationsByUser(user.sub)
          .subscribe(({ data }) => {
            this.reservations = data;
          });
      }
    });
  }

  onOpenAddAttendees(reservationId: number) {
    const opensModal = this.isOpenAddAttendees();
    opensModal.set(reservationId, true);
  }

  onCloseAddAttendees(reservationId: number) {
    const opensModal = this.isOpenAddAttendees();
    opensModal.set(reservationId, false);
    this.loadReservations();
  }

  cancel(id : number) {
    this.reservationService.cancelReservation(id).subscribe({});
    this.loadReservations();
  }
}
