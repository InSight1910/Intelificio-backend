import {Component} from '@angular/core';
import {ModalComponent} from '../modal/modal.component';
import {from, map, Observable, of, tap} from 'rxjs';
import {CommonSpace} from '../../../../shared/models/commonspace.model';
import {CommonModule} from '@angular/common';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {AppState} from '../../../../states/intelificio.state';
import {Store} from '@ngrx/store';
import {selectCommunity} from '../../../../states/community/community.selectors';
import {CommonSpaceService} from '../../../services/commonspace/commonspace.service';
import {Reservation} from "../../../../shared/models/reservation.model";
import {User} from "../../../../shared/models/user.model";
import {selectUser} from "../../../../states/auth/auth.selectors";

@Component({
  selector: 'app-home-space',
  standalone: true,
  imports: [ModalComponent, CommonModule, ReactiveFormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeSpaceComponent {
  constructor(
    private fb: FormBuilder,
    private commonSpaceService: CommonSpaceService,
    private store: Store<AppState>
  ) {
    this.form = this.fb.group({
      date: ['', Validators.required],
      startTime: ['', Validators.required],
      endTime: [''],
    });
    this.store.select(selectUser).subscribe(x => this.loguedUser = x)
  }

  form: FormGroup;
  loguedUser: User | null = {} as User;
  selectedSpace: CommonSpace = {} as CommonSpace;
  isCreatingReservation: boolean = false;
  modalTitle: string = 'Create Reservation';
  buttonTitle!: string;
  isModalOpen: boolean = false;
  canMakeReservation: boolean = false;
  months: { month: string; monthNumber: number; year: number }[] = [];
  days: string[] = [
    'Domingo',
    'Lunes',
    'Martes',
    'Miércoles',
    'Jueves',
    'Viernes',
    'Sábado',
  ];

  commonSpaces$!: CommonSpace[];

  ngOnInit() {
    this.getMonths();
    this.generateCalendar();
    this.loadCommonSpace();
  }

  loadCommonSpace() {
    this.store.select(selectCommunity).subscribe((community) => {
      this.commonSpaceService
        .getCommonSpacesByCommunity(community?.id!)
        .pipe(
          (
            tap(({data}) => {
              this.selectedSpace = data[0];
            }),
              map(({data}) => data))
        ).subscribe(
        {
          next: (data) => {
            this.selectedSpace = data[0];
            this.commonSpaces$ = data;
          }
        }
      );
    });
  }

  onChangeInput() {
    console.log(this.form.value);
  }

  onClickCloseEdit() {
    this.isModalOpen = false;
  }

  onClickCreateReservation(id: number) {
    this.isModalOpen = true;
    this.buttonTitle = "Reservar";
    this.modalTitle = `Reservar ${this.selectedSpace?.name}`;
  }

  onSubmit(event: Event | void) {
    if (event) event.preventDefault()
    const reservation: Reservation = {
      userId: this.loguedUser?.sub!,
      commonSpaceId: this.selectedSpace?.id!,
      date: this.form.get("date")?.value!,
      startTime: this.form.get("startTime")?.value,
      endTime: this.form.get("endTime")?.value
    }

    console.log(reservation)
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
    for (let i = 1; i <= 3; i++) {
      const nextMonth = new Date(today.getFullYear(), today.getMonth() + i, 1);
      const value = {
        month: nextMonth.toLocaleString('es-CL', {month: 'long'}),
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
  }

  nextMonth() {
    if (this.indexMonth === 2) {
      return;
    }
    this.indexMonth++;
    this.generateCalendar();
  }

  generateCalendar(): void {
    const {monthNumber, year} = this.months.at(this.indexMonth)!;

    const firstDayOfMonth = new Date(year, monthNumber, 1).getDay(); // Day of the week of the 1st day
    const lastDateOfMonth = new Date(year, monthNumber + 1, 0).getDate(); // Last date of the month

    this.daysInMonth = Array(firstDayOfMonth).fill(null); // Fill empty slots before 1st day
    for (let day = 1; day <= lastDateOfMonth; day++) {
      this.daysInMonth.push(day);
    }
  }
}
