import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import { ReservationService } from '../../../services/reservation/reservation.service';
import {DatePipe, TitleCasePipe} from "@angular/common";
import {MyReservation, ReservationStatus} from "../../../../shared/models/reservation.model";

@Component({
  selector: 'app-confirm-reservation',
  standalone: true,
  imports: [
    DatePipe,
    TitleCasePipe
  ],
  templateUrl: './confirm-reservation.component.html',
  styleUrl: './confirm-reservation.component.css',
})
export class ConfirmReservationComponent implements OnInit {
  token!: string;
  reservationId!: number;
  reservation: MyReservation = {
    date: new Date('2024-10-11'),
    startTime: new Date('2024-10-11T15:00:00'),
    endTime: new Date('2024-10-11T17:00:00'),
    status: ReservationStatus.PENDIENTE,
    spaceName: "",
    attendees: 10,
    id: 1,
    location: "",
  };
  message: string | null = null;
  error: string | null = null;
  confirmed: boolean = false;
  loading: boolean = false;

  constructor(
    private router: ActivatedRoute,
    private route: Router,
    private reservationService: ReservationService,
    private service : ReservationService,
  ) {
  }

  ngOnInit() {
    this.loading = true;
    this.confirmed = true;
    const reservationId  = this.router.snapshot.queryParamMap.get('reservationId');
    const token = this.router.snapshot.queryParamMap.get('token');


    if (reservationId != null && token != null) {
      this.reservationId = parseInt(reservationId) ?? 0;
      this.token = decodeURIComponent(token);
    }

    if (this.token != null) {
      this.getDetail(this.reservationId);
    }
  }

  getDetail(id: number) {
    this.service.getReservationsById(id).subscribe((response) => {
      this.reservation = response.data;
      this.confirmed = false;
      this.loading = false;
    });
  }

  confirm(){
    this.reservationService
      .confirmReservation(this.reservationId, this.token)
      .subscribe({
        next: (response) => {
          if( response.status === 202){
            this.confirmed = true;
            this.message = "Reserva confirmada con éxito.";
            setTimeout(() => {
              this.confirmed = false;
              this.message = null;
              this.route.navigate(['/MisReservas']).then((r) => {});
            },3000);
          }
        },
        error: (error) => {
         if( error.status === 400 ){
           this.error = error.error[0].message;
           setTimeout(() => {
             this.error = null;
             this.route.navigate(['/MisReservas']).then((r) => {});
           },5000);
         }
        },
      });
  }

  toReserva(){
    this.route.navigate(['/EspacioComun']).then((r) => {});
  }

  cancel(){
    console.log(this.reservationId)
    this.service.cancelReservation(this.reservationId).subscribe({
      next: (response) => {
        if (response.status === 204){
          this.message = "Reserva Cancelada con éxito.";
          setTimeout(() => {
            this.confirmed = false;
            this.message = null;
            this.route.navigate(['/MisReservas']).then((r) => {});
          },3000);
        }
      }
    })
  }

  protected readonly ReservationStatus = ReservationStatus;
}
