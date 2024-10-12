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
import {User} from "../../../../shared/models/user.model";
import {selectUser} from "../../../../states/auth/auth.selectors";
import {Store} from "@ngrx/store";
import {AppState} from "../../../../states/intelificio.state";
import {Community} from "../../../../shared/models/community.model";
import {selectCommunity} from "../../../../states/community/community.selectors";
import {FormatRutDirective} from "../../../../shared/directives/format-rut.directive";
import { FormatRutPipe } from '../../../../shared/pipes/format-rut.pipe';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-singup',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule,FormatRutDirective,FormatRutPipe],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css',
})
export class SingupComponent implements OnInit {
  constructor(private service: AuthService,private store: Store<AppState>) {}

  notification = false;
  notificationMessage = '';
  IsSuccess = false;
  IsError = false;
  waiting = false;
  notificationSuccess = 'Usuario creado exitosamente';
  selectedFile: File | null = null;
  selectedFileName = '';
  isFileUploaded = false;
  user!: User | null;
  community!: Community | null;
  errores: string[] = [];

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
        Validators.maxLength(15),
        Validators.minLength(12),
        this.phoneValidator(),
      ]),
      rut: new FormControl('', [Validators.required, this.rutValidator()]),
      rol: new FormControl('', Validators.required),
      birthDate: new FormControl('', [
        Validators.required,
        this.birthDateValidator(),
      ]),
    },
  );

  ngOnInit(): void {
    this.getRoles();
    this.store.select(selectUser).subscribe(
      (user: User | null) => {
        this.user = user;
      });

    this.store.select(selectCommunity).subscribe(
      (community: Community | null) => {
        this.community = community;
      });
  }


  onSubmit() {
    if (this.selectedFile) {
      const formData: FormData = new FormData();
      formData.append('file', this.selectedFile!);
      formData.append('creatorID', `${this.user?.sub!}`);
      formData.append('communityID', `${this.community?.id!}`);
      this.waiting = true;

      this.service
        .signupMassive(formData)
        .pipe(
          tap((response) => {
            if (response.status === 202) {
              this.notificationMessage = "Carga recibida, se informará a su correo el resultado."
              this.IsSuccess = true;
              this.notification = true;
              this.waiting = false;
              setTimeout(() => {
                this.notification = false;
                this.IsSuccess = false;
                this.clean();
              }, 5000);
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
            phoneNumber: this.signupForm.value.phoneNumber?.replace(/\s+/g, '') ?? '',
            password: "#3st@cl@v4Sol1S4dS8r@",
            rut: this.signupForm.value.rut?.replace(/[.\-]/g, '').toUpperCase() ?? '',
            role: this.signupForm.value.rol ?? '',
            birthDate: this.signupForm.value.birthDate ?? '',
          },
          Users: [],
          CreatorID: this.user?.sub ?? 0,
          CommunityID : this.community?.id ?? 0,
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
              }, 3000);
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
        rut: '',
        rol: '',
        birthDate: '',
      },
      { emitEvent: false }
    );
    this.signupForm.enable();
    this.selectedFile = null;
    this.selectedFileName = '';
    this.notificationMessage = "";
    this.notification = false;
    this.IsError = false;
    this.IsSuccess = false;
    this.isFileUploaded = false;
    this.errores = [];

    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }

  }

  closeNotification() {
    this.notification = false;
    this.IsSuccess = false;
    this.IsError = true;
    this.selectedFile = null;
    this.isFileUploaded = false;
    this.selectedFileName = '';
    this.notificationMessage = "";
    this.errores = [];

    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }

  }

  getRoles() {
    this.service.getAllRole().subscribe(
      (response: { data: Role[] }) => {
        this.listaRol = response.data;
      },
      (error) => {
       if(error.status === 0){
         console.error('Error al obtener Roles, compruebe que el backEnd responde.', );
       }
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
      return valid ? null : { prohoneprefix: true };
    };
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
      this.selectedFile = input.files![0];
      this.selectedFileName = input.files![0].name;
      this.isFileUploaded = true;

      this.notificationMessage = "";
      this.errores = [];
      this.notification = false;
      this.IsError = false;
      this.IsSuccess = false;
      const reader: FileReader = new FileReader();

      reader.onload = (e: any) => {
        const data = new Uint8Array(e.target.result);
        const workbook = XLSX.read(data, { type: 'array' });
        const sheetName = workbook.SheetNames[0];
        const worksheet = workbook.Sheets[sheetName];
        const jsonData = XLSX.utils.sheet_to_json(worksheet, { header: 1 });
        const isValid = this.validateExcelData(jsonData);
        if (!isValid) {
          this.notification = true;
          this.IsSuccess = false;
          this.IsError = true;
          this.selectedFile = null;
          input.value = '';
          this.selectedFileName = '';
          this.isFileUploaded = false;
          this.signupForm.disable();
          return;
        }
        this.signupForm.disable();
      };
      reader.readAsArrayBuffer(this.selectedFile);
      this.signupForm.disable();
    }
  }

  validateExcelData(data: any[]): boolean {
    const errores: string[] = [];

    if (!data || data.length <= 1) {
      this.notificationMessage = 'El archivo Excel está vacío';
      return false;
    }

    const cleanedData = data.filter(row => row.some((cell: any) => cell && String(cell).trim() !== ''));
    const requiredColumns = ['Rut', 'Nombre', 'Apellido', 'Correo', 'Telefono', 'FechaNacimiento', 'Rol'];
    const fileColumns = cleanedData[0];

    const missingColumns = requiredColumns.filter(col => !fileColumns.includes(col));
    if (missingColumns.length > 0) {
      this.notificationMessage = `Faltan las columnas: ${missingColumns.join(', ')}`;
      return false;
    }

    const rutIndex = fileColumns.indexOf('Rut');
    const nombreIndex = fileColumns.indexOf('Nombre');
    const apellidoIndex = fileColumns.indexOf('Apellido');
    const correoIndex = fileColumns.indexOf('Correo');
    const telefonoIndex = fileColumns.indexOf('Telefono');
    const fechaNacimientoIndex = fileColumns.indexOf('FechaNacimiento');
    const rolIndex = fileColumns.indexOf('Rol');


    const rutSet = new Set();
    const correoSet = new Set();
    const telefonoSet = new Set();

    for (let i = 1; i < cleanedData.length; i++) {
      const row = cleanedData[i];


      const rut = row[rutIndex];
      if (!rut || String(rut).trim() === '') {
        errores.push(`Fila ${i + 1}: El RUT está vacío.`);
      } else if (!this.isValidRut(String(rut))) {
        errores.push(`Fila ${i + 1}: El RUT ${rut} no es válido.`);
      } else if (rutSet.has(rut)) {
        errores.push(`Fila ${i + 1}: El RUT ${rut} está repetido.`);
      }
      rutSet.add(rut);


      const nombre = row[nombreIndex];
      if (!nombre || String(nombre).trim() === '') {
        errores.push(`Fila ${i + 1}: El Nombre está vacío.`);
      }


      const apellido = row[apellidoIndex];
      if (!apellido || String(apellido).trim() === '') {
        errores.push(`Fila ${i + 1}: El Apellido está vacío.`);
      }


      const correo = row[correoIndex];
      if (!correo || String(correo).trim() === '') {
        errores.push(`Fila ${i + 1}: El Correo está vacío.`);
      } else {
        const correoStr = String(correo).trim();
        const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
        if (!emailRegex.test(correoStr)) {
          errores.push(`Fila ${i + 1}: El Correo ${correoStr} no es válido.`);
        } else if (correoSet.has(correoStr)) {
          errores.push(`Fila ${i + 1}: El Correo ${correoStr} está repetido.`);
        }
        correoSet.add(correoStr);
      }


      const telefono = row[telefonoIndex];
      if (!telefono || String(telefono).trim() === '') {
        errores.push(`Fila ${i + 1}: El Teléfono está vacío.`);
      } else if (telefonoSet.has(telefono)) {
        errores.push(`Fila ${i + 1}: El Teléfono ${telefono} está repetido.`);
      }
      telefonoSet.add(telefono);


      const fechaNacimiento = row[fechaNacimientoIndex];
      if (!fechaNacimiento || String(fechaNacimiento).trim() === '') {
        errores.push(`Fila ${i + 1}: La Fecha de Nacimiento está vacía`);
      }

      const rol = row[rolIndex];
      if (!rol || String(rol).trim() === '') {
        errores.push(`Fila ${i + 1}: El Rol está vacío.`);
      } else {
        const rolUsuario = String(rol).trim().toLowerCase();
        const rolValido = this.listaRol.some((r: Role) => r.name.toLowerCase() === rolUsuario);
        if (!rolValido) {
          errores.push(`Fila ${i + 1}: El Rol "${rol}" no es válido. Debe ser uno de los siguientes: ${this.listaRol.map(r => r.name).join(', ')}`);
        }
      }
    }

    if (errores.length > 0) {
      this.errores = errores.slice(0, 5);
      if (errores.length > 5) {
        this.errores.push('...existen más filas con errores o falta de datos, por favor revise la planilla e intente nuevamente.');
      }
      return false;
    }

    return true;
  }




  isValidRut(rut: string): boolean {
    if (!rut) {
      return false;
    }
    const cleanRut = rut.replace(/[.\-]/g, '').toUpperCase();
    if (!/^[0-9]+[K0-9]$/.test(cleanRut)) {
      return false;
    }
    const body = cleanRut.slice(0, -1);
    const dv = cleanRut.slice(-1).toUpperCase();

    if (body.length < 7) {
      return false;
    }

    const calculatedDV = this.calculateDV(body);
    return dv === calculatedDV;
  }

  downloadModel() {
    const ws_data = [
      ['Rut', 'Nombre', 'Apellido', 'Correo', 'Telefono', 'FechaNacimiento', 'Rol'], // Encabezados
    ];
    const ws: XLSX.WorkSheet = XLSX.utils.aoa_to_sheet(ws_data);
    const roles = this.listaRol.map(role => role.name);

    roles.forEach(role => {
      ws_data.push(['', '', '', '', '', '', role]);
    });

    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'PlantillaUsuarios');

    const wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });

    const blob = new Blob([wbout], { type: 'application/octet-stream' });
    const link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    link.setAttribute('download', 'CreacionMasivaUsuarios.xlsx');
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }
}
