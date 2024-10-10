import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ReservationService } from '../../../services/reservation/reservation.service';

@Component({
  selector: 'app-confirm-reservation',
  standalone: true,
  imports: [],
  templateUrl: './confirm-reservation.component.html',
  styleUrl: './confirm-reservation.component.css',
})
export class ConfirmReservationComponent {
  token!: string;
  reservationId!: number;
  invalidToken: boolean = false;
  errorInConfirmation: boolean = false;

  constructor(
    private router: ActivatedRoute,
    private reservationService: ReservationService
  ) {
    router.queryParams.subscribe((params) => {
      this.reservationId = params['reservationId'];
      this.token = params['token'];
    });
  }

  ngOnInit() {
    this.reservationService
      .confirmReservation(this.reservationId, this.token)
      .subscribe({
        next: () => {
          console.log('Reservation confirmed');
        },
        error: (error) => {
          console.log(error.error);
          if (
            error.status === 400 &&
            error.error[0]?.code == 'Confirmation.ConfirmationTokenNotCorrect'
          ) {
            this.invalidToken = true;
          } else {
            this.errorInConfirmation = true;
          }
        },
      });
  }
}
