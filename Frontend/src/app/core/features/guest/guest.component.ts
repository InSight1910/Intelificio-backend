import { Component, Input } from '@angular/core';
import { GuestService } from '../../services/guest/guest.service';
import { Guest } from '../../../shared/models/guest.model';
import { CommonModule } from '@angular/common';
import { AddModalComponent } from './add-modal/add-modal.component';
import { EditModalComponent } from './edit-modal/edit-modal.component';
import { Store } from '@ngrx/store';
import { AppState } from '../../../states/intelificio.state';
import { selectCommunity } from '../../../states/community/community.selectors';

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
  currentDateTime: string;
  isLoading = false;
  communityId!: number;
  communityName!: string;
  
  constructor(
    private guestService: GuestService,
    private store: Store<AppState>
  ) {
    this.currentDateTime = this.getCurrentDateTime();
  }

  ngOnInit(): void {
    setInterval(() => {
      this.currentDateTime = this.getCurrentDateTime();
    }, 1000);
    this.store.select(selectCommunity).subscribe((community) => {
      this.communityId = community?.id!;
      this.loadGuest(+this.communityId);
    });
  }

  private getCurrentDateTime(): string {
    const now = new Date();
    return now.toLocaleString();
  }

  loadGuest(communityId: number) {
    this.isLoading = true;
    this.guestService.getAllGuestsByCommunity(communityId).subscribe((data) => {
      this.isLoading = false;     
      this.guests = data.data;
    });
  }

  editGuest(updated: boolean): void {
    if (updated) {
      this.loadGuest(+this.communityId);
    }
  }

  updateGuestList(updated: boolean): void {
    if (updated) {
      this.loadGuest(+this.communityId);
    }
  }

  updateCurrentDateTime(): void {
    setInterval(() => {
      const now = new Date();
      this.currentDateTime = now.toLocaleString();
    }, 1000);
  }

  isOpenEdit: boolean = false;
  selectGuestId: number = 0;

  onClickOpenEdit(guestId: number){
    this.isOpenEdit = true;
    this.selectGuestId = guestId;
  }

}
