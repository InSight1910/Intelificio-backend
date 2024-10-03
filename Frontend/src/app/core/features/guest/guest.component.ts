import { Component, Input } from '@angular/core';
import { GuestService } from '../../services/guest/guest.service';
import { Guest } from '../../../shared/models/guest.model';
import { CommonModule } from '@angular/common';
import { AddModalComponent } from './add-modal/add-modal.component';
import { EditModalComponent } from './edit-modal/edit-modal.component';

@Component({
  selector: 'app-guest',
  standalone: true,
  imports: [CommonModule, AddModalComponent, EditModalComponent],
  templateUrl: './guest.component.html',
  styleUrls: ['./guest.component.css']
})
export class GuestComponent {
  @Input() unitId: string = '0';
  guests: Guest[] = [];
  currentDateTime: string = '';
  isLoading = false;
  CommunityName: any;

  constructor(private guestService: GuestService) {}

  ngOnChanges(): void {
    if (this.unitId != '0') {
      this.loadGuest(+this.unitId);
    }
  }

  loadGuest(unitId: number) {
    this.isLoading = true;
    this.guestService.getAllGuestsByUnit(unitId).subscribe((data) => {
      this.isLoading = false;
      this.guests = data.data;
    });
  }

  editGuest(updated: boolean): void {
    if (updated) {
      this.loadGuest(+this.unitId);
    }
  }

  updateGuestList(updated: boolean): void {
    if (updated) {
      this.loadGuest(+this.unitId);
    }
  }

  updateCurrentDateTime(): void {
    setInterval(() => {
      const now = new Date();
      this.currentDateTime = now.toLocaleString();
    }, 1000);
  }

}
