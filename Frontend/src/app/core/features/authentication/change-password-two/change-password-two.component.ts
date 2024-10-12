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
import {ActivatedRoute, Router} from '@angular/router';
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
  new: boolean = false;

  constructor(
    private fb: FormBuilder,
    private router: ActivatedRoute,
    private route: Router,
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
      { validators: [this.passwordMatchValidator, Validators.required] }
    );
    router.queryParams.subscribe((params) => {
      this.email = params['email'];
      this.token = params['token'];
      this.new = params['new'];
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
              if(this.new){
                this.message = 'Contraseña creada exitosamente.';
              } else {
                this.message = 'Cambio de contraseña exitoso';
              }
              this.errors = null;
              setTimeout(() => {
                this.route.navigate(['/login']).then((r) => {});
              },5000);
            }
          }),
          catchError((error) => {
            this.errors = error.error.errors;
            if(error)
            this.message = "No se logró realizar el cambio de clave, intente nuevamente el proceso";
            setTimeout(() => {
              this.route.navigate(['/login']).then((r) => {});
            },5000);
            return of(error);
          })
        )
        .subscribe();
    }
  }
}
