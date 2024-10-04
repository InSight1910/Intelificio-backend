import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../../services/auth/auth.service';
import { catchError, of, tap } from 'rxjs';

@Component({
  selector: 'app-change-password-one',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './change-password-one.component.html',
  styleUrl: './change-password-one.component.css',
})
export class ChangePasswordOneComponent implements OnInit {
  form: FormGroup;
  message: string | null = null;
  errors: string[] | null = null;
  send = false;
  waiting = false;

  constructor(private authService: AuthService, private fb: FormBuilder) {
    this.form = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
      },
      Validators.required
    );
  }

  ngOnInit(): void {
    this.send = false;
  }

  onSubmit(event: Event) {
    this.waiting = true;
    event.preventDefault();
    if (this.form.valid) {
      this.authService
        .forgotPassword(this.form.value.email)
        .pipe(
          tap((response) => {
            console.log(response);
            if (response.status === 204) {
              this.message =
                'Se ha enviando un correo con las instrucciones para cambiar la contraseña';
              this.send = true;
              this.waiting = false;
            }
          }),
          catchError((error) => {
            console.error(error);
            if (error.status === 0) {
              this.errors = ['Hubo un error de nuestra parte.'];
              this.waiting = false;
            }
            return of(error);
          })
        )
        .subscribe();
    }
  }
}
