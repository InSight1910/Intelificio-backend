import { CommonModule } from '@angular/common';
import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Store} from "@ngrx/store";
import {AppState} from "../../../states/intelificio.state";
import {
  AbstractControl,
  FormBuilder, FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators
} from "@angular/forms";
import {User} from "../../../shared/models/user.model";
import {selectUser} from "../../../states/auth/auth.selectors";
import {AuthService} from "../../services/auth/auth.service";
import {UpdateUser} from "../../../shared/models/auth.model";
import {NavbarComponent} from "../../../shared/component/navbar/navbar.component";
import {AuthActions} from "../../../states/auth/auth.actions";



@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit{
  isLoading: boolean = false;
  notification: boolean = false;
  IsSuccess: boolean = false;
  IsError: boolean = false;
  notificationMessage: string = "";
  @Output() close = new EventEmitter<void>();


  form: FormGroup;
  user!: User | null;

  constructor(private store: Store<AppState>,private fb: FormBuilder, private service : AuthService, private navbar: NavbarComponent) {
    this.form = this.fb.group({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required,Validators.email]),
      phoneNumber: new FormControl('', [
        Validators.required,
        Validators.maxLength(15),
        Validators.minLength(12),
        this.phoneValidator(),
      ]),
    });
  }

  ngOnInit(){
   this.store.select(selectUser).subscribe(
     (user: User | null) => {
       this.user = user;
       this.form.controls['firstName'].setValue(this.user?.firstName ?? "");
       this.form.controls['lastName'].setValue(this.user?.lastName ?? "");
       this.form.controls['email'].setValue(this.user?.email ?? "");
       this.form.controls['phoneNumber'].setValue(this.user?.phoneNumber ?? "");
     }
   );
  }

  onClose() {
    this.close.emit();
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

  OnSubmit(){
    this.isLoading = true;
    if(this.form.valid){
      const updateUser: UpdateUser = {
        firstName: this.form.controls['firstName'].value,
        lastName: this.form.controls['lastName'].value,
        phoneNumber: this.form.controls['phoneNumber'].value,
        email: this.form.controls['email'].value,
        token: localStorage.getItem('token') ?? "",
        refreshToken: localStorage.getItem('refreshToken') ?? ""
      };

      this.service.updateUser(updateUser).subscribe({
        next:(response) =>{
          if(response.status === 200){
            this.isLoading = true;
            var token = response.body.data.token;
            localStorage.removeItem('token');
            localStorage.removeItem('refreshToken');
            localStorage.setItem('token',token);
            localStorage.setItem('refreshToken',response.body.data.refreshToken);
            this.store.dispatch(AuthActions.updateToken({token}));
            this.notificationMessage =
              'Datos actualizados correctamente';
            this.IsSuccess = true;
            this.notification = true;
            setTimeout(() => {
              this.notification = false;
              this.IsSuccess = false;
              this.isLoading = false;
              this.close.emit();

            }, 5000);
          }
        },
        error: (error) =>{
          this.isLoading = false;
          this.notificationMessage = 'No fue posible actualizar sus datos';
          this.IsError = true;
          this.notification = true;
          setTimeout(() => {
            this.notification = false;
            this.IsError = false;
            this.store.dispatch(AuthActions.logout());
          }, 5000);
        }
      });

    }
  }

}
