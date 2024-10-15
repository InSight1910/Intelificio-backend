import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/component/navbar/navbar.component';
import { MenuComponent } from './shared/component/menu/menu.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    ReactiveFormsModule,
    NavbarComponent,
    MenuComponent,
    CommonModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'Intelificio';
  openNavbar = false;
  showMenu: boolean = false;

  constructor(private router: Router) {}

  ngOnInit() {
    this.router.events.subscribe(() => {
      const url = this.router.url.split('?')[0];
      switch (url) {
        case '/login':
        case '/forgot-password':
        case '/change-password':
        case '/select-community':
        case '/ConfirmarCorreo':
        case '/print-notification':
          this.showMenu = false;
          break;
        default:
          this.showMenu = true;
      }
    });
  }
}
