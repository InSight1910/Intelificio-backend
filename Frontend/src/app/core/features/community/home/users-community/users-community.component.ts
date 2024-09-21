import { Component, Input } from '@angular/core';
import { UsersCommunity } from '../../../../../shared/models/community.model';
import { CommunityService } from '../../../../services/community/community.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-users-community',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './users-community.component.html',
  styleUrl: './users-community.component.css',
})
export class UsersCommunityComponent {
  @Input() communityId = '0';
  isLoading: boolean = false;

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
}
