import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import {AuthService} from "../../../services/auth/auth.service";
import {ConfirmEmail} from "../../../../shared/models/auth.model";


@Component({
  selector: 'app-confirm-email',
  standalone: true,
  imports: [ CommonModule],
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.css',
})
export class ConfirmEmailComponent implements OnInit {

  status = signal<'loading' | 'success' | 'error' | 'already-confirmed'| 'user-not-found' | 'invalid-token'>('loading');
  correo: string = "";
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: AuthService,
  ) {}

  ngOnInit(): void {
    const email = this.route.snapshot.queryParamMap.get('email');
    const token = this.route.snapshot.queryParamMap.get('token');

    this.correo = email ?? "";

    if (!email || !token) {
      this.status.set('error');
      return;
    }
    const confirmEmail: ConfirmEmail = {
      email: email,
      token: token,
    }

    this.service.confirmEmail(confirmEmail).subscribe({
      next: (response) => {
        if (response.status === 200){
          const email = response.body.data.email || 'defaultEmail';
          const token = response.body.data.token|| 'defaultToken';
          this.status.set('success');
          setTimeout(() => {
            this.router.navigate(['/change-password'], {
              queryParams: {new: true, email: email, token: token}
            }).then(r => {});
          },3000);
        } else {
          this.status.set('error');
        }
      },
      error: (error) => {
        if (error.status === 400) {
          switch (error.error?.[0].code) {
            case 'Authentication.ConfirmEmail.EmailNotSendOnConfirmEmail':
              this.status.set('user-not-found');
              break;
            case 'Authentication.ConfirmEmail.UserAlreadyConfirmThisEmailOnOnConfirmEmail':
              this.status.set('already-confirmed');
              break;
            case 'Authentication.ConfirmEmail.InvalidToken':
              this.status.set('invalid-token');
              break;
            default:
              this.status.set('error');
              break;
          }
        } else {
          this.status.set('error');
        }
      }
    });
  }

  handleLogin(): void {
    this.router.navigate(['/login']).then((r) => {});
  }


}
