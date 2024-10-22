import { Component, EventEmitter, Output } from '@angular/core';
import { UnitService } from '../../../services/unit/unit.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { catchError, of, tap } from 'rxjs';
import { CreateUnit, UnitType } from '../../../../shared/models/unit.model';
import { BuildingService } from '../../../services/building/building.service';
import { Building } from '../../../../shared/models/building.model';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../states/intelificio.state';
import { selectCommunity } from '../../../../states/community/community.selectors';
import { UserService } from '../../../services/user/user.service';

@Component({
  selector: 'app-add-modal',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './add-modal.component.html',
  styleUrl: './add-modal.component.css',
})
export class AddModalComponent {
  @Output() addUnitEvent = new EventEmitter<boolean>();
  unitForm: FormGroup;
  userForm: FormGroup;
  isOpen: boolean = false;
  errors!: { message: string }[] | null;
  isAdding: boolean = false;
  isSuccess: boolean = false;
  canAddUnit: boolean = false;
  types: UnitType[] = [];
  buildings: Building[] = [];
  floors: number[] = [];
  isSearching: boolean = false;
  canSearch: boolean = true;
  canAddUser: boolean = false;

  constructor(
    private store: Store<AppState>,
    private unitService: UnitService,
    private userService: UserService,
    private buildingService: BuildingService,
    private fb: FormBuilder
  ) {
    this.unitForm = this.fb.group({
      id: [''],
      floor: [''],
      number: [''],
      surface: [''],
      building: [''],
      unitType: [''],
    });
    this.userForm = this.fb.group({
      id: [''],
      rut: [''],
      name: [''],
      role: [''],
    });
    console.log('UnitForm:', this.unitForm.value);
    console.log('UserForm:', this.userForm.value);
  }

  ngOnInit() {
    this.unitService.getTypes().subscribe((types) => {
      this.types = types.data;
    });

    this.store.select(selectCommunity).subscribe((community) => {
      this.buildingService
        .getbyCommunityId(community?.id!)
        .subscribe((buildings) => {
          this.buildings = buildings.data;
        });
    });
  }

  // ngOnInit() {
  //   this.unitService.getTypes().subscribe((types) => {
  //     this.types = types.data;
  //   });

  //   this.store.select(selectCommunity).subscribe((community) => {
  //     if (community?.id) {
  //       console.log('ID de la comunidad:', community.id);

  //       // Obtener edificios de la comunidad
  //       this.buildingService.getbyCommunityId(community.id).subscribe((buildings) => {
  //         this.buildings = buildings.data;
  //       });

  //       // Obtener usuarios de la comunidad
  //       this.userService.getAllByCommunity(community.id).subscribe((users) => {
  //         console.log('Usuarios obtenidos:', users.data);  // <- Verifica si se obtienen los usuarios
  //         this.users = users.data;
  //       }, (error) => {
  //         console.error('Error al cargar los usuarios:', error);  // <- Para ver si hay algún error
  //       });
  //     }
  //   });
  // }

  onChangeBuilding() {
    const building = this.buildings.find(
      (x) => x.id == this.unitForm.get('building')?.value
    )!;
    this.floors = Array.from(
      { length: building.floors },
      (_, index) => index + 1
    );
  }

  // onChangeUser() {
  //   const selectedUserId = this.unitForm.get('user')?.value;
  //   console.log('ID de usuario seleccionado:', selectedUserId);  // <- Verificar qué ID de usuario se selecciona

  //   const user = this.users.find((x) => x.id == selectedUserId);
  //   console.log('Usuario seleccionado:', user);  // <- Verificar el usuario seleccionado
  // }

  onClickSearch() {
    this.errors = null;
    this.isSearching = true;
    const rut = this.userForm.get('rut')?.value;
    console.log('RUT ingresado:', rut);

    if (rut == '' || rut == null) {
      console.log('El RUT está vacío o es nulo.');
      this.isSearching = false;
      this.errors = [{ message: 'Debe ingresar un rut!' }];
      return;
    }
    console.log('Llamando a getByRut con el RUT:', rut);
    this.userService
      .getByRut(rut)
      .pipe(
        tap((response) => {
          console.log('Respuesta del servicio:', response);
          this.userForm.patchValue({
            id: response.data.id,
            name: response.data.name,
            role: response.data.role,
          });
          this.canAddUser = true;
          this.isSearching = false;
          console.log(
            'Datos del usuario actualizados en el formulario:',
            this.userForm.value
          );
        }),

        catchError((error) => {
          console.error('Error al obtener el usuario:', error);
          this.errors = [
            { message: 'Usuario no encontrado o no pertenece a la comunidad' },
          ];
          this.unitForm.patchValue({ name: '', role: '', id: '' });
          this.canAddUser = false;
          this.isSearching = false;
          return of(error);
        })
      )
      .subscribe();
  }

  onClickAddUnit() {
    this.isAdding = true;
    const unit: CreateUnit = {
      floor: this.unitForm.get('floor')?.value,
      number: this.unitForm.get('number')?.value,
      surface: this.unitForm.get('surface')?.value,
      buildingId: this.unitForm.get('building')?.value,
      unitTypeId: this.unitForm.get('unitType')?.value,
      userId: this.userForm.get('id')?.value,
    };

    this.unitService
      .createUnit(unit)
      .pipe(
        tap(() => {
          this.isSuccess = true;
          this.isAdding = false;
          setTimeout(() => {
            this.isSuccess = false;
          }, 2000);

          this.unitForm.reset({
            floor: '',
            number: '',
            surface: '',
            user: '',
            building: '',
            unitType: '',
          });

          this.floors = [];
          this.errors = null;

          this.addUnitEvent.emit(true);
        }),
        catchError((error) => {
          this.canAddUnit = false;
          this.isAdding = false;
          this.errors = error.error;
          return of(error);
        })
      )
      .subscribe();
  }

  onClickOpenModal() {
    this.isOpen = true;
  }

  onClickCloseModal() {
    this.isOpen = false;
    this.errors = null;
    this.unitForm.reset({
      floor: '',
      number: '',
      surface: '',
      user: '',
      building: '',
      unitType: '',
      id: '',
    });
    this.userForm.reset({
      id: '',
      rut: '',
      name: '',
      role: '',
    });
  }
}
