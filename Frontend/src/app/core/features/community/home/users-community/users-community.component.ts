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
  isDeleting: boolean = false;

  constructor(private communityService: CommunityService) {}

  users: UsersCommunity[] = [];

  ngOnInit() {
    this.isLoading = true;
    this.communityService
      .getUsersByCommunity(+this.communityId)
      .subscribe((users) => {
        this.isLoading = false;
        this.users = users.data;
      });
  }

  onClickDeleting() {}
}
