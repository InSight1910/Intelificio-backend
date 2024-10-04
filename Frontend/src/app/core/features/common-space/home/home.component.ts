import {
  Component,
  DEFAULT_CURRENCY_CODE,
  Pipe,
  PipeTransform,
  signal,
} from '@angular/core';
import { ModalComponent } from '../modal/modal.component';
import { map, tap } from 'rxjs';
import { CommonSpace } from '../../../../shared/models/commonspace.model';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AppState } from '../../../../states/intelificio.state';
import { Store } from '@ngrx/store';
import { selectCommunity } from '../../../../states/community/community.selectors';
import { CommonSpaceService } from '../../../services/commonspace/commonspace.service';
import {
  CountReservation,
  CreateReservation,
  ListReservation,
  ReservationStatus,
} from '../../../../shared/models/reservation.model';
import { User } from '../../../../shared/models/user.model';
import { selectUser } from '../../../../states/auth/auth.selectors';
import { ReservationService } from '../../../services/reservation/reservation.service';
import { MessageComponent } from '../../../../shared/component/error/message.component';

@Pipe({
  standalone: true,
  name: 'isSelected',
})
export class IsSelectedPipe implements PipeTransform {
  transform(map: Map<number, boolean>, key: number): boolean {
    console.log(!!map.get(key));
    return !!map.get(key);
  }
}

@Component({
  selector: 'app-home-space',
  standalone: true,
  imports: [
    ModalComponent,
    CommonModule,
    ReactiveFormsModule,
    MessageComponent,
    IsSelectedPipe,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeSpaceComponent {
  [x: string]: any;
  constructor(
    private fb: FormBuilder,
    private commonSpaceService: CommonSpaceService,
    private reservationService: ReservationService,
    private store: Store<AppState>
  ) {
    this.form = this.fb.group({
      date: ['', Validators.required],
      startTime: ['', Validators.required],
      endTime: [''],
    });
    this.store.select(selectUser).subscribe((x) => (this.loguedUser = x));
  }
  ReservationStatus = ReservationStatus;
  form: FormGroup;
  errors: { message: string }[] = [];
  onSuccess: string = '';
  loguedUser: User | null = {} as User;
  selectedSpace: CommonSpace = {} as CommonSpace;
  isCreatingReservation: boolean = false;
  modalTitle: string = 'Create Reservation';
  buttonTitle!: string;
  isModalOpen: boolean = false;
  canMakeReservation: boolean = false;
  months: {
    month: string;
    monthNumber: number;
    year: number;
  }[] = [];
  days: string[] = [
    'Lunes',
    'Martes',
    'Miércoles',
    'Jueves',
    'Viernes',
    'Sábado',
    'Domingo',
  ];

  commonSpaces$!: CommonSpace[];
  reservationsCounts: CountReservation[] = [];
  reservations: ListReservation[] = [];
  isLoadingReservation: boolean = false;

  isModalOpenReservations = signal(new Map<number, boolean>());

  ngOnInit() {
    this.getMonths();
    this.generateCalendar();
    this.loadCommonSpace();
    this.loadCountReservations();
  }

  loadCommonSpace() {
    this.store.select(selectCommunity).subscribe((community) => {
      this.commonSpaceService
        .getCommonSpacesByCommunity(community?.id!)
        .pipe(
          (tap(({ data }) => {
            this.selectedSpace = data[0];
          }),
          map(({ data }) => data))
        )
        .subscribe({
          next: (data) => {
            this.selectedSpace = data[0];
            this.commonSpaces$ = data;
          },
        });
    });
  }

  loadReservations(day: number) {
    this.isLoadingReservation = true;
    this.store.select(selectCommunity).subscribe((community) => {
      const { year, monthNumber } = this.months.at(this.indexMonth)!;
      this.reservationService
        .getReservationsByCommunityAndMonth(
          community?.id!,
          new Date(year, monthNumber, day)
        )
        .subscribe({
          next: ({ data }) => {
            this.reservations = data;
            this.isLoadingReservation = false;
          },
        });
    });
  }

  onClickCloseEdit() {
    this.isModalOpen = false;
  }

  titleDetail: string = 'Reservas del día';
  onClickShowReservations(day: number) {
    this.loadReservations(day);
    this.titleDetail = `Reservas del día ${day}/${
      this.months.at(this.indexMonth)?.monthNumber
    }`;
    const opensModal = this.isModalOpenReservations();
    opensModal.set(day, true);
  }

  onClickCloseReservations(day: number) {
    console.log(day);
    const opensModal = this.isModalOpenReservations();
    opensModal.set(day, false);
    this.reservations = [];
  }

  onClickCreateReservation(id: number) {
    this.isModalOpen = true;
    this.buttonTitle = 'Reservar';
    this.modalTitle = `Reservar ${this.selectedSpace?.name}`;
  }

  onSubmit(event: Event | void) {
    if (event) event.preventDefault();
    if (this.form.invalid) {
      return;
    }
    const reservation: CreateReservation = {
      userId: this.loguedUser?.sub!,
      commonSpaceId: this.selectedSpace?.id!,
      date: this.form.get('date')?.value!,
      startTime: this.form.get('startTime')?.value,
      endTime: this.form.get('endTime')?.value,
    };

    this.reservationService.create(reservation).subscribe({
      next: ({ data }) => {
        this.onSuccess = 'Reserva creada con éxito';
      },
      error: ({ error }) => {
        this.errors = error;
        setTimeout(() => {
          this.errors = [];
        }, 3000);
        this.form.reset();
      },
    });
  }

  loadCountReservations() {
    this.store.select(selectCommunity).subscribe((community) => {
      const selectedMonth = this.months.at(this.indexMonth)!;
      this.reservationService
        .getCountReservationsByCommunityAndMonth(
          community?.id!,
          selectedMonth.year,
          selectedMonth.monthNumber
        )
        .subscribe({
          next: ({ data }) => {
            this.reservationsCounts = data;
          },
        });
    });
  }

  onChange(event: Event) {
    const value = (event.target as HTMLInputElement).value;

    this.selectedSpace = this.commonSpaces$.find(
      (space) => space.id === parseInt(value)
    )!;
    if (value == '') {
      this.canMakeReservation = false;
      return;
    }
    this.canMakeReservation = true;
    return;
  }

  getMonths() {
    const months = [];
    const today = new Date();
    for (let i = 0; i < 3; i++) {
      const nextMonth = new Date(today.getFullYear(), today.getMonth() + i, 1);
      const value = {
        month: nextMonth.toLocaleString('es-CL', { month: 'long' }),
        monthNumber: nextMonth.getMonth(),
        year: nextMonth.getFullYear(),
      };

      months.push(value);
    }
    this.months = months;
  }

  indexMonth: number = 0;
  daysInMonth!: number[];

  prevMonth() {
    if (this.indexMonth === 0) {
      return;
    }
    this.indexMonth--;
    this.generateCalendar();
    this.reservationsCounts = [];
    this.reservationsCounts = [];
    this.loadCountReservations();
  }

  nextMonth() {
    if (this.indexMonth === 3) {
      return;
    }
    this.indexMonth++;
    this.generateCalendar();
    this.reservationsCounts = [];
    this.reservationsCounts = [];
    this.loadCountReservations();
  }

  generateCalendar(): void {
    const { monthNumber, year } = this.months.at(this.indexMonth)!;

    const firstDayOfMonth = new Date(year, monthNumber, 1).getDay() + (6 % 7);
    const lastDateOfMonth = new Date(year, monthNumber + 1, 0).getDate();

    this.daysInMonth = Array(firstDayOfMonth).fill(null);
    for (let day = 1; day <= lastDateOfMonth; day++) {
      this.daysInMonth.push(day);
    }
  }
  getTimeAsDate(time: Date): Date {
    const currentDate = new Date();
    console.log(time);
    const [hours, minutes, seconds] = time.toString().split(':').map(Number);
    currentDate.setHours(hours, minutes, seconds);
    return currentDate;
  }
}
