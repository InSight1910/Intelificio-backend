import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import { AppState } from '../../../states/intelificio.state';
import { Store } from '@ngrx/store';
import { selectTitle } from '../../../states/navbar/navbar.selectors';
import { CommonModule } from '@angular/common';
import { User } from '../../models/user.model';
import { selectUser } from '../../../states/auth/auth.selectors';
import { selectCommunity } from '../../../states/community/community.selectors';
import { AuthActions } from '../../../states/auth/auth.actions';
import { ProfileComponent } from '../../../core/features/profile/profile.component';
import { ModalComponent } from '../../../core/features/common-space/modal/modal.component';
import {
  ReactiveFormsModule,
} from '@angular/forms';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    CommonModule,
    ProfileComponent,
    ModalComponent,
    ReactiveFormsModule,
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit{
  @Output() openNavbar: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private store: Store<AppState>) {}
  title!: string;
  user!: User | null;
  communityName!: string;
  navbarDisplay: boolean = false;
  isModalOpen: boolean = false;
  isDarkTheme: boolean = false;

  ngOnInit() {

    const savedTheme = sessionStorage.getItem('theme') || 'system';
    this.isDarkTheme = savedTheme === 'dark';
    this.applyTheme(this.isDarkTheme);

    this.store.select(selectTitle).subscribe((title) => {
      this.title = title;
    });

    this.store.select(selectUser).subscribe(user =>{
      this.user = user;
    });

    this.store.select(selectCommunity).subscribe((community) => {
      if (community) {
        this.communityName = community?.name!;
      }
    });
  }

  togleNavbar() {
    this.navbarDisplay = !this.navbarDisplay;
    this.openNavbar.emit(this.navbarDisplay);
  }

  openProfileModal() {
    this.isModalOpen = true;
  }

  onClickCloseEdit() {
    this.isModalOpen = false;
  }

  logout() {
    this.store.dispatch(AuthActions.logout());
  }

  toggleTheme() {
    this.isDarkTheme = !this.isDarkTheme;
    this.applyTheme(this.isDarkTheme);
  }

  applyTheme(isDarkTheme: boolean) {
    if (isDarkTheme) {
      document.documentElement.setAttribute('data-theme', 'dark');
      sessionStorage.setItem('theme', 'dark');
    } else {
      document.documentElement.setAttribute('data-theme', 'light');
      sessionStorage.setItem('theme', 'light');
    }
  }

}
