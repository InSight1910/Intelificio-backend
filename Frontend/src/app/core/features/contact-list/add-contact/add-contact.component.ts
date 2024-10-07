import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule, ValidationErrors,
  ValidatorFn,
  Validators
} from "@angular/forms";
import {Store} from "@ngrx/store";
import {AppState} from "../../../../states/intelificio.state";
import {CommunityService} from "../../../services/community/community.service";
import {LocationService} from "../../../services/location/location.service";
import {AdminCommunityComponent} from "../../community/adminCommunity/admin-community.component";
import {NgClass} from "@angular/common";
import {Contact} from "../../../../shared/models/contact.model";
import {ContactService} from "../../../services/contact/contact.service";
import {AuthActions} from "../../../../states/auth/auth.actions";
import {ContactListComponent} from "../contact-list.component";

@Component({
  selector: 'app-add-contact',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './add-contact.component.html',
  styleUrl: './add-contact.component.css'
})
export class AddContactComponent {
  IsLoading: boolean = false;
  notification: boolean = false;
  IsSuccess: boolean = false;
  IsError: boolean = false;
  notificationMessage: string = "";
  @Output() close = new EventEmitter<void>();
  @Input() CommunityID: number = 0;

  form: FormGroup;

  constructor(private fb: FormBuilder,
              private service: ContactService,
              private contactList: ContactListComponent) {
    this.form = this.fb.group({
      FirstName: new FormControl('', Validators.required),
      LastName: new FormControl('', Validators.required),
      Email: new FormControl('', [Validators.email]),
      PhoneNumber: new FormControl('', [Validators.required,
        Validators.maxLength(15),
        Validators.minLength(12),
        this.phoneValidator(),]),
      Service: new FormControl('', Validators.required),
      CommunityID: new FormControl(0),
    });
  }


  onClose() {
    this.close.emit();
  }

  OnSubmit(){
    this.IsLoading = true;
    if (this.form.valid){
      const createContact  = {
        FirstName: this.form.value.FirstName,
        LastName:this.form.value.LastName,
        Email: this.form.value.Email ? this.form.value.Email : 'Sin@correo.cl',
        PhoneNumber: this.form.value.PhoneNumber.replace(/\s+/g, ''),
        Service:  this.form.value.Service,
        CommunityID: this.CommunityID,
      };

      this.service.create(createContact).subscribe(
        {
          next: (response) => {
            if (response.status === 204) {
              this.IsLoading = false;
              this.notificationMessage =
                'Contacto agregado satisfactoriamente.';
              this.IsSuccess = true;
              this.notification = true;
              setTimeout(() => {
                this.notification = false;
                this.IsSuccess = false;
                this.IsLoading = false;
                this.contactList.getContacts();
                this.close.emit();
              }, 3000);
            }
          },
          error: (error) =>{
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
              this.notificationMessage = 'No fue posible guardar este contacto';
              this.IsError = true;
              this.notification = true;
              setTimeout(() => {
                this.notification = false;
                this.notificationMessage = '';
                this.IsError = false;
              }, 5000);
            }
          }
        }
      );

    }
  }

  closeNotification(){
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

}
