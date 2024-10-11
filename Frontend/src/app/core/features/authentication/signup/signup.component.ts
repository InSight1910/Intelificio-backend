import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  ValidatorFn,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Role } from '../../../../shared/models/role.model';
import { AuthService } from '../../../services/auth/auth.service';

import { SignupDTO } from '../../../../shared/models/signUpCommand.model';
import { catchError, of, tap } from 'rxjs';
import { FormatPhoneDirective } from '../../../../shared/directives/format-phone/format-phone.directive';
import { FormatPhonePipe } from '../../../../shared/pipes/format-phone/format-phone.pipe';

@Component({
  selector: 'app-singup',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    FormatPhoneDirective,
    FormatPhonePipe,
  ],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css',
})
export class SingupComponent implements OnInit {
  constructor(private service: AuthService) {}

  notification = false;
  notificationMessage = '';
  IsSuccess = false;
  IsError = false;
  waiting = false;
  notificationSuccess = 'Usuario creado exitosamente';
  selectedFile: File | null = null;
  selectedFileName = '';
  isFileUploaded = false;

  listaRol: Role[] = [
    {
      name: 'Administrador',
      id: 1,
    },
    {
      name: 'Usuario',
      id: 2,
    },
  ];

  signupForm = new FormGroup(
    {
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      phoneNumber: new FormControl('', [
        Validators.required,
        Validators.maxLength(11),
        Validators.minLength(9),
        this.phoneValidator(),
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(8),
        this.passwordValidator(),
      ]),
      confirmPassword: new FormControl('', Validators.required),
      rut: new FormControl('', [Validators.required, this.rutValidator()]),
      rol: new FormControl('', Validators.required),
      birthDate: new FormControl('', [
        Validators.required,
        this.birthDateValidator(),
      ]),
    },
    { validators: this.passwordMatchValidator }
  );

  ngOnInit(): void {
    this.getRoles();
  }

  onSubmit() {
    if (this.selectedFile) {
      const formData: FormData = new FormData();
      formData.append('file', this.selectedFile!);
      formData.append('test', 'test');

      this.service
        .signupMassive(formData)
        .pipe(
          tap((response) => {
            if (response.status === 204) {
              this.notification = true;
              setTimeout(() => {
                this.notification = false;
                this.clean();
              }, 3000);
            }
          }),
          catchError((error) => {
            console.error('Error: ', error);
            return of(error);
          })
        )
        .subscribe();
    } else {
      if (this.signupForm.valid) {
        const signupDTO: SignupDTO = {
          User: {
            firstName: this.signupForm.value.firstName ?? '',
            lastName: this.signupForm.value.lastName ?? '',
            email: this.signupForm.value.email ?? '',
            phoneNumber:
              '+56' +
              (this.signupForm.value.phoneNumber?.replace(/\s+/g, '') ?? ''),
            password: this.signupForm.value.password ?? '',
            rut:
              this.signupForm.value.rut?.replace(/[.\-]/g, '').toUpperCase() ??
              '',
            role: this.signupForm.value.rol ?? '',
            birthDate: this.signupForm.value.birthDate ?? '',
          },
          Users: [],
        };
        this.waiting = true;
        this.signupForm.disable();
        this.service.signup(signupDTO).subscribe({
          next: (response) => {
            if (response.status === 204) {
              this.notificationMessage = this.notificationSuccess;
              this.IsSuccess = true;
              this.notification = true;
              this.waiting = false;
              setTimeout(() => {
                this.notification = false;
                this.IsSuccess = false;
                this.clean();
                this.signupForm.enable();
              }, 5000);
            }
          },
          error: (error) => {
            this.waiting = false;
            if (error.status === 400) {
              const errorData = error.error?.[0];
              if (errorData?.code === 'Authentication.SignUp.AlreadyCreated') {
                this.notificationMessage = errorData.message;
                this.IsError = true;
                this.notification = true;
                setTimeout(() => {
                  this.notification = false;
                  this.IsError = false;
                  this.signupForm.controls['email'].setValue('');
                  this.signupForm.enable();
                }, 5000);
              } else {
                console.log('Error 400 no controlado:', error);
              }
            } else {
              console.log('Error no controlado:', error);
            }
          },
        });
      }
    }
  }

  clean() {
    this.signupForm.reset(
      {
        firstName: '',
        lastName: '',
        email: '',
        phoneNumber: '',
        password: '',
        confirmPassword: '',
        rut: '',
        rol: '',
        birthDate: '',
      },
      { emitEvent: false }
    );
    this.signupForm.enable();
    this.selectedFile = null;
    this.selectedFileName = '';
    this.isFileUploaded = false;
  }

  closeNotification() {
    this.notification = false;
  }

  getRoles() {
    this.service.getAllRole().subscribe(
      (response: { data: Role[] }) => {
        this.listaRol = response.data;
      },
      (error) => {
        console.error('Error al obtener edificios', error);
      }
    );
  }

  onInputChange(controlName: string): void {
    const control = this.signupForm.get(controlName);
    if (control) {
      control.markAsUntouched();
    }
  }

  phoneValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }
      const validPrefixes = [
        '+54', // Argentina
        '+591', // Bolivia
        '+56', // Chile
        '+57', // Colombia
        '+506', // Costa Rica
        '+53', // Cuba
        '+593', // Ecuador
        '+503', // El Salvador
        '+502', // Guatemala
        '+509', // Haití
        '+504', // Honduras
        '+52', // México
        '+505', // Nicaragua
        '+507', // Panamá
        '+595', // Paraguay
        '+51', // Perú
        '+1', // República Dominicana (aunque +1 también es el prefijo de EE.UU. y Canadá, se incluye aquí para la República Dominicana)
        '+598', // Uruguay
        '+58', // Venezuela
      ];

      const valid = validPrefixes.some((prefix) =>
        control.value.startsWith(prefix)
      );
      // return valid ? null : { prohoneprefix: true };
      return null;
    };
  }

  passwordValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (!value) {
        return null;
      }

      const passwordPattern =
        /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$/;

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

  birthDateValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const birthDate = new Date(control.value);
      const today = new Date();

      if (isNaN(birthDate.getTime())) {
        return { invalidDate: true };
      }

      const twoYearsAgo = new Date();
      twoYearsAgo.setFullYear(today.getFullYear() - 2);

      return birthDate <= twoYearsAgo ? null : { tooRecent: true };
    };
  }

  rutValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const rut = control.value;

      if (!rut) {
        return null;
      }

      const cleanRut = rut.replace(/[\.\-]/g, '').toUpperCase();

      if (!/^[0-9]+[K0-9]$/.test(cleanRut)) {
        return { invalidRut: true };
      }

      const body = cleanRut.slice(0, -1);
      const dv = cleanRut.slice(-1).toUpperCase();

      if (body.length < 7) {
        return { invalidRut: true };
      }

      // Validar el dígito verificador
      const calculatedDV = this.calculateDV(body);
      return dv === calculatedDV ? null : { invalidRut: true };
    };
  }

  calculateDV(rut: string): string {
    let sum = 0;
    let multiplier = 2;

    for (let i = rut.length - 1; i >= 0; i--) {
      sum += +rut.charAt(i) * multiplier;
      multiplier = multiplier < 7 ? multiplier + 1 : 2;
    }

    const remainder = 11 - (sum % 11);

    if (remainder === 11) {
      return '0';
    } else if (remainder === 10) {
      return 'K';
    } else {
      return remainder.toString();
    }
  }

  onChangeFile(event: Event) {
    const input = event.target as HTMLInputElement;

    if (input.files?.length) {
      this.signupForm.disable();
      this.signupForm.reset();

      this.selectedFile = input.files![0];
      this.selectedFileName = input.files![0].name;
      this.isFileUploaded = true;
    }
  }
}
