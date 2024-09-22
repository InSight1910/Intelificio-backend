import { Component , OnInit} from '@angular/core';
import { FormControl, FormGroup,Validators,ReactiveFormsModule} from '@angular/forms';
import { CommonModule } from '@angular/common'; 
import { Role } from '../../../../shared/models/role.model';
import { AuthService } from '../../../services/auth/auth.service';
import { RouterState } from '@angular/router';

@Component({
  selector: 'app-singup',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './singup.component.html',
  styleUrl: './singup.component.css'
})
export class SingupComponent implements OnInit {

  constructor(
    private service: AuthService
  ){}

  listaRol: Role[] =
  [
    {
      "name": "Administrador",
      "id": 1
    },
    {
      "name": "Usuario",
      "id": 2
    }
  ];

  singupForm = new FormGroup({
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    phoneNumber: new FormControl('', [Validators.required]),
    password: new FormControl('', Validators.required),
    rut: new FormControl('', Validators.required),
    rol: new FormControl('', Validators.required),
    birthDate: new FormControl('', Validators.required),
  });

  ngOnInit(): void {
    this.getRoles();
  }

  onSubmit(){
    if (this.singupForm.valid){

      const singupDTO = {
        FirstName: this.singupForm.controls['firstName'].value,
        LastName: this.singupForm.controls['lastName'].value,
        Email: this.singupForm.controls['email'].value,
        PhoneNumber: this.singupForm.controls['phoneNumber'].value,
        Password: this.singupForm.controls['password'].value,
        Rut: this.singupForm.controls['rut'].value,
        rol: this.singupForm.controls['rol'].value,
        BirthDate: this.singupForm.controls['birthDate'].value
      };

      this.service.singup(singupDTO).subscribe({
        next: (response) => {
          if (response.status === 200){
              console.log("Funciona")
          }
        },
        error: (error) => {
            console.log('Error:', error);
          }
      })

    } 
    
  }

  private cleanFields(){
    this.singupForm.controls['firstName'].setValue('');
    this.singupForm.controls['lastName'].setValue('');
    this.singupForm.controls['email'].setValue('');
    this.singupForm.controls['phoneNumber'].setValue('');
    this.singupForm.controls['password'].setValue('');
    this.singupForm.controls['rut'].setValue('');
    this.singupForm.controls['rol'].setValue('');
    this.singupForm.controls['birthDate'].setValue('');
  }

  getRoles() {
    this.service.getAllRole().subscribe(
      (response: {data: Role[]}) => {
       this.listaRol = response.data;
    },
    (error) => {
      console.error('Error al obtener edificios', error);
    }
    );
  }

}
