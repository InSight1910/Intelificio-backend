import { Component } from '@angular/core';
import { ModalComponent } from '../modal/modal.component';
import { from, map, Observable, of, tap } from 'rxjs';
import { CommonSpace } from '../../../../shared/models/commonspace.model';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AppState } from '../../../../states/intelificio.state';
import { Store } from '@ngrx/store';
import { selectCommunity } from '../../../../states/community/community.selectors';
import { CommonSpaceService } from '../../../services/commonspace/commonspace.service';

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
      date: [''],
      startTime: [''],
      endTime: [''],
    });
  }

  form: FormGroup;
  selectedSpace: CommonSpace = {} as CommonSpace;
  isCreatingReservation: boolean = false;
  modalTitle: string = 'Create Reservation';
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

  commonSpaces$!: Observable<CommonSpace[]>;

  ngOnInit() {
    this.getMonths();
    this.generateCalendar();
    this.loadCommonSpace();
  }

  loadCommonSpace() {
    this.store.select(selectCommunity).subscribe((community) => {
      this.commonSpaces$ = this.commonSpaceService
        .getCommonSpacesByCommunity(community?.id!)
        .pipe(
          (tap(({ data }) => {
            this.selectedSpace = data[0];
            console.log(data);
          }),
          map(({ data }) => data))
        );
      // this.commonSpaces$.subscribe();
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
    this.commonSpaces$.subscribe((commonSpaces) => {
      const commonSpace = commonSpaces.find((space) => space.id === id);
      this.modalTitle = `Reservar ${commonSpace?.name}`;
    });
  }

  onChange(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.commonSpaces$.subscribe((commonSpaces) => {
      this.selectedSpace = commonSpaces.find(
        (space) => space.id === parseInt(value)
      )!;
    });
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
        month: nextMonth.toLocaleString('es-CL', { month: 'long' }),
        monthNumber: nextMonth.getMonth(),
        year: nextMonth.getFullYear(),
      };

      months.push(value);
    }
    this.months = months;
  }

  currentMonth: string = '';
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
    const { monthNumber, year } = this.months.at(this.indexMonth)!;

    const firstDayOfMonth = new Date(year, monthNumber, 1).getDay(); // Day of the week of the 1st day
    const lastDateOfMonth = new Date(year, monthNumber + 1, 0).getDate(); // Last date of the month

    this.daysInMonth = Array(firstDayOfMonth).fill(null); // Fill empty slots before 1st day
    for (let day = 1; day <= lastDateOfMonth; day++) {
      this.daysInMonth.push(day);
    }

    console.log(this.daysInMonth);
  }
}
