import { Component } from '@angular/core';
import { ModalComponent } from '../modal/modal.component';
import { from, Observable, of } from 'rxjs';
import { CommonSpace } from '../../../../shared/models/commonspace.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home-space',
  standalone: true,
  imports: [ModalComponent, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeSpaceComponent {
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
  commonSpaces$: Observable<CommonSpace[]> = of([
    {
      id: 1,
      name: 'Meeting Room',
      capacity: 100,
      location: '1st floor',
      communityId: 1,
      isInMaintenance: true,
    },
    {
      id: 2,
      name: 'Conference Room',
      capacity: 200,
      location: '2nd floor',
      communityId: 1,
      isInMaintenance: false,
    },
  ] as CommonSpace[]);

  ngOnInit() {
    this.getMonths();
    this.generateCalendar();
  }

  onClickCloseEdit() {
    this.isModalOpen = false;
  }

  onClickCreateReservation(id: number) {
    console.log(id);
    this.isModalOpen = true;
    this.commonSpaces$.subscribe((commonSpaces) => {
      const commonSpace = commonSpaces.find((space) => space.id === id);

      this.modalTitle = `Reservar ${commonSpace?.name}`;
    });
  }

  onChange(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    console.log(value);
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
