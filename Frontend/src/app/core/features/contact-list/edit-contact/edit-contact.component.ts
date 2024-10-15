import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ContactService } from '../../../services/contact/contact.service';

import { Contact } from '../../../../shared/models/contact.model';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-edit-contact',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, NgClass],
  templateUrl: './edit-contact.component.html',
  styleUrl: './edit-contact.component.css',
})
export class EditContactComponent implements OnInit {
  IsLoading: boolean = false;
  notification: boolean = false;
  IsSuccess: boolean = false;
  IsError: boolean = false;
  notificationMessage: string = '';
  @Output() close = new EventEmitter<void>();
  @Input() ContactEdited: Contact = {
    communityID: 0,
    email: 's',
    firstName: '',
    id: 0,
    lastName: '',
    phoneNumber: '',
    service: '',
  };

  form: FormGroup;

  constructor(private fb: FormBuilder, private service: ContactService) {
    this.form = this.fb.group({
      id: new FormControl(0),
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.email]),
      phoneNumber: new FormControl('', [
        Validators.required,
        Validators.maxLength(15),
        Validators.minLength(12),
        this.phoneValidator(),
      ]),
      service: new FormControl('', Validators.required),
      communityID: new FormControl(0),
    });
  }

  ngOnInit() {
    this.form.patchValue(this.ContactEdited);
  }

  onClose() {
    this.close.emit();
  }

  OnSubmit() {
    this.IsLoading = true;
    if (this.form.valid) {
      const EditContact = {
        FirstName: this.form.value.firstName,
        LastName: this.form.value.lastName,
        Email: this.form.value.email ? this.form.value.email : 'Sin@correo.cl',
        PhoneNumber: this.form.value.phoneNumber.replace(/\s+/g, ''),
        Service: this.form.value.service,
        CommunityID: this.form.value.communityID,
      };

      this.service.update(this.form.value.id, EditContact).subscribe({
        next: (response) => {
          if (response.status === 200) {
            this.IsLoading = false;
            this.notificationMessage =
              'Contacto actualizado satisfactoriamente.';
            this.IsSuccess = true;
            this.notification = true;
            setTimeout(() => {
              this.notification = false;
              this.IsSuccess = false;
              this.IsLoading = false;
              this.close.emit();
            }, 5000);
          }
        },
        error: (error) => {
          this.IsLoading = false;
          if (error.status === 400) {
            const errorData = error.error?.[0];
            this.notificationMessage = errorData.message;
            this.IsError = true;
            this.notification = true;
            setTimeout(() => {
              this.notification = false;
              this.notificationMessage = '';
              this.IsError = false;
            }, 5000);
          } else {
            this.notificationMessage =
              'No fue posible actualizar este contacto';
            this.IsError = true;
            this.notification = true;
            setTimeout(() => {
              this.notification = false;
              this.notificationMessage = '';
              this.IsError = false;
            }, 5000);
          }
        },
      });
    }
  }

  closeNotification() {
    this.notification = false;
    this.IsSuccess = false;
    this.IsError = false;
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
      return valid ? null : { prohoneprefix: true };
    };
  }

  onInputChange(controlName: string): void {
    const control = this.form.get(controlName);
    if (control) {
      control.markAsUntouched();
    }
  }

  delete(contactoID: number) {
    this.IsLoading = true;
    this.service.delete(contactoID).subscribe({
      next: (response) => {
        if (response.status === 200) {
          this.IsLoading = false;
          this.notificationMessage = 'Contacto eliminado satisfactoriamente.';
          this.IsSuccess = true;
          this.notification = true;
          setTimeout(() => {
            this.notification = false;
            this.IsSuccess = false;
            this.IsLoading = false;

            this.close.emit();
          }, 3000);
        }
      },
      error: (error) => {
        this.IsLoading = false;
        if (error.status === 400) {
          const errorData = error.error?.[0];
          this.notificationMessage = errorData.message;
          this.IsError = true;
          this.notification = true;
          setTimeout(() => {
            this.notification = false;
            this.notificationMessage = '';
            this.IsError = false;
          }, 5000);
        } else {
          this.notificationMessage = 'No fue posible eliminar este contacto';
          this.IsError = true;
          this.notification = true;
          setTimeout(() => {
            this.notification = false;
            this.notificationMessage = '';
            this.IsError = false;
          }, 5000);
        }
      },
    });
  }
}
