import { Component } from '@angular/core';
import { AppState } from '../../../states/intelificio.state';
import { Store } from '@ngrx/store';
import { selectTitle } from '../../../states/navbar/navbar.selectors';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { User } from '../../models/user.model';
import { selectUser } from '../../../states/auth/auth.selectors';
import { Community } from '../../models/community.model';
import { selectCommunity } from '../../../states/community/community.selectors';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  constructor(private store: Store<AppState>) {}
  title!: string;
  user!: Observable<User | null>;
  communityName!: string;
  navbarDisplay: boolean = false;

  ngOnInit() {
    this.store.select(selectTitle).subscribe((title) => {
      this.title = title;
    });

    this.user = this.store.select(selectUser);
    console.log(this.user);

    this.store.select(selectCommunity).subscribe((community) => {
      if (community) {
        this.communityName = community?.name!;
      }
    });
  }

  togleNavbar() {
    this.navbarDisplay = !this.navbarDisplay;
  }
}
