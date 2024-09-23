import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';
import { catchError, of, tap } from 'rxjs';

@Component({
  selector: 'app-change-password-two',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './change-password-two.component.html',
  styleUrl: './change-password-two.component.css',
})
export class ChangePasswordTwoComponent {
  form: FormGroup;
  message: string | null = null;
  errors: string[] | null = null;
  email!: string;
  token!: string;

  constructor(
    private fb: FormBuilder,
    private router: ActivatedRoute,
    private authService: AuthService
  ) {
    this.form = this.fb.group(
      {
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(8),
            this.passwordValidator(),
          ],
        ],
        confirmPassword: ['', Validators.required],
      },
      { validators: this.passwordMatchValidator }
    );
    router.queryParams.subscribe((params) => {
      this.email = params['email'];
      this.token = params['token'];
    });
  }

  passwordValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (!value) {
        return null;
      }

      const passwordPattern = /^(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$/;

      const valid = passwordPattern.test(value);

      return valid ? null : { invalidPassword: true };
    };
  }
  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    var password = control.get('password')?.value;
    if (password.length <= 0) {
      password = 1;
    }
    const confirmPassword = control.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordsDoNotMatch: true };
  }
  onInputChange(controlName: string): void {
    const control = this.form.get(controlName);
    if (control) {
      control.markAsUntouched();
    }
  }

  onSubmit(event: Event) {
    event.preventDefault();
    if (this.form.valid) {
      this.authService
        .changePassword(this.email, this.token, this.form.value.password)
        .pipe(
          tap((response) => {
            if (response.ok) {
              this.message = 'Cambio de contraseña exitoso';
              this.errors = null;
            }
          }),
          catchError((error) => {
            this.errors = error.error.errors;
            this.message = null;
            return of(error);
          })
        )
        .subscribe();
    }
  }
}
