import { Component, Input } from '@angular/core';
import {
  Community,
  UsersCommunity,
} from '../../../../../shared/models/community.model';
import { CommunityService } from '../../../../services/community/community.service';
import { CommonModule } from '@angular/common';
import { AddUserModalComponent } from './add-user-modal/add-user-modal.component';
import { User } from '../../../../../shared/models/user.model';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../../states/intelificio.state';
import { selectCommunity } from '../../../../../states/community/community.selectors';
import { tap } from 'rxjs';

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

  constructor(
    private communityService: CommunityService,
    private store: Store<AppState>
  ) {}

  users: UsersCommunity[] = [];
  userToBeDeleted: User | null = null;
  activateModal: boolean = false;
  community: Community | null = null;

  ngOnInit() {
    this.isLoading = true;
    this.loadCommunity();
  }

  loadCommunity() {
    this.store
      .select(selectCommunity)
      .pipe(
        tap((community) => {
          this.community = community;
          this.communityService
            .getUsersByCommunity(community?.id!)
            .subscribe((users) => {
              this.isLoading = false;
              this.users = users.data;
            });
        })
      )
      .subscribe();
  }

  updateList(updated: boolean) {
    if (updated) {
      this.loadCommunity();
    }
  }

  onClickDeleting() {
    const userId = this.userToBeDeleted?.sub!;
    this.isDeleting[userId] = true;
    this.communityService
      .deleteUserFromCommunity(+this.community?.id!, userId)
      .subscribe(() => {
        this.users = this.users.filter((user) => user.id !== userId);
        this.isDeleting[userId] = false;
        this.loadCommunity();
        this.closeModal();
      });
  }

  openmodal({ email, id, name, role,firstName,lastName,phoneNumber }: UsersCommunity) {
    this.userToBeDeleted = {
      role,
      given_name: name,
      sub: id,
      email,
      firstName: '',
      lastName: '',
      phoneNumber

    };
    this.activateModal = true;
  }

  closeModal() {
    this.activateModal = false;
  }
}
