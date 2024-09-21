import { Component, Input } from '@angular/core';
import { UsersCommunity } from '../../../../../shared/models/community.model';
import { CommunityService } from '../../../../services/community/community.service';
import { CommonModule } from '@angular/common';
import { AddUserModalComponent } from './add-user-modal/add-user-modal.component';

@Component({
  selector: 'app-users-community',
  standalone: true,
  imports: [CommonModule, AddUserModalComponent],
  templateUrl: './users-community.component.html',
  styleUrl: './users-community.component.css',
})
export class UsersCommunityComponent {
  @Input() communityId = '0';
  isLoading: boolean = false;
  isDeleting: { [key: number]: boolean } = {};

  constructor(private communityService: CommunityService) {}

  users: UsersCommunity[] = [];

  ngOnInit() {
    this.isLoading = true;
    this.loadCommunity();
  }

  loadCommunity() {
    this.communityService
      .getUsersByCommunity(+this.communityId)
      .subscribe((users) => {
        this.isLoading = false;
        this.users = users.data;
      });
  }

  updateList(updated: boolean) {
    if (updated) {
      this.loadCommunity();
    }
  }

  onClickDeleting(userId: number) {
    this.isDeleting[userId] = true;
    this.communityService
      .deleteUserFromCommunity(+this.communityId, userId)
      .subscribe(() => {
        this.users = this.users.filter((user) => user.id !== userId);
        this.isDeleting[userId] = false;
        this.loadCommunity();
      });
  }
}
