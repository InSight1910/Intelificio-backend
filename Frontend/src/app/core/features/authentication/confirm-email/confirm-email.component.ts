import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from '../../../services/notification/notification.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-confirm-email',
  standalone: true,
  imports: [ CommonModule],
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.css',
})
export class ConfirmEmailComponent implements OnInit {

  status = signal<'loading' | 'success' | 'error'>('loading');

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: NotificationService
  ) {}

  ngOnInit(): void {
    const email = this.route.snapshot.queryParamMap.get('email');
    const token = this.route.snapshot.queryParamMap.get('token');

    if (!email || !token) {
      this.status.set('error');
      return;
    }
    this.service.confirmEmail(email, token).subscribe({
      next: () => this.status.set('success'),
      error: () => this.status.set('error'),
    });
  }

  handleLogin(): void {
    this.router.navigate(['/login']).then((r) => {});
  }

}
