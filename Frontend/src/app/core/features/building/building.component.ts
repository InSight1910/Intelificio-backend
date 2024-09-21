import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup,Validators,ReactiveFormsModule} from '@angular/forms';
import { BuildingService } from '../../services/building.service';
import { Building } from '../../../shared/models/building.model';

@Component({
  selector: 'app-building',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './building.component.html',
  styleUrl: './building.component.css'
})
export class BuildingComponent implements OnInit {
    
    Edificios: Building[] = [{
      id: 1, 
      name: 'Edificio Test 1', 
      floors: 1, 
      units: 10, 
      communityId: 1, 
      communityName: 'Comunidad de prueba' 
    },
    {
      id: 2, 
      name: 'Edificio Test 2', 
      floors: 1, 
      units: 10, 
      communityId: 1, 
      communityName: 'Comunidad de prueba' 
    }];

    Edificio: Building = {
      id: 1, 
      name: 'Edificio Test 1', 
      floors: 1, 
      units: 10, 
      communityId: 1, 
      communityName: 'Comunidad de prueba' 
    };
    
    buildingForm = new FormGroup({
      nombreEdificio: new FormControl('', Validators.required),
      pisosEdificio: new FormControl(0 , [Validators.required, Validators.min(1)]),
      ComunidadEdificio: new FormControl('', Validators.required),
      CantidadUnidades: new FormControl(0, Validators.required)
    });

    isEdited = false;
    isDeletion = false;
    notification = false;
    isCreation = false;
    postUpdateOrCreate = false;
    selectedBuildingId: number = 1;
    createdMessage: string = 'Edificio creado.';
    updatedMessage: string = 'Edificio actualizado.';
    
    constructor(
      private service: BuildingService
    ){}

    ngOnInit(): void {
        this.populateFields();
        this.getNumberOfBuilding();
        this.disableFields();
    }

    async getNumberOfBuilding(){
      this.service.getbyCommunityId(2).subscribe(
        (response: {data: Building[]}) => {
            this.Edificios = response.data;
            if (this.Edificios.length > 0) {
              this.selectedBuildingId = this.Edificios[0].id;
              this.detail(this.selectedBuildingId);
            }
        },
        (error) => {
          console.error('Error al obtener edificios', error);
        }
      );
    }

    private populateFields(): void {
      this.buildingForm.controls['nombreEdificio'].setValue(this.Edificio.name);
      this.buildingForm.controls['pisosEdificio'].setValue(this.Edificio.floors);
      this.buildingForm.controls['CantidadUnidades'].setValue(this.Edificio.units);
      this.buildingForm.controls['ComunidadEdificio'].setValue(this.Edificio.communityName);
    }

    private disableFields(){
      this.buildingForm.controls['nombreEdificio'].disable();
      this.buildingForm.controls['pisosEdificio'].disable();
      this.buildingForm.controls['CantidadUnidades'].disable();
      this.buildingForm.controls['ComunidadEdificio'].disable();
    }

    private enableFields(){
      this.buildingForm.controls['nombreEdificio'].enable();
      this.buildingForm.controls['pisosEdificio'].enable();
    }

    private cleanFields(){
      this.buildingForm.controls['nombreEdificio'].setValue('');
      this.buildingForm.controls['nombreEdificio'].enable();
      this.buildingForm.controls['pisosEdificio'].setValue(0);
      this.buildingForm.controls['pisosEdificio'].enable();
      this.buildingForm.controls['CantidadUnidades'].setValue(0);
      this.buildingForm.controls['CantidadUnidades'].disable();
    }

    detail(id: number){
      this.selectedBuildingId = id; 
      this.isEdited = false;
      this.isCreation = false;
      this.notification = false;
      const selectedBuilding = this.Edificios?.find(building => building.id === id);
  
      if (selectedBuilding) {
        this.Edificio = selectedBuilding;
        this.populateFields();
        this.disableFields();
      } else {
        console.error('Edificio no encontrado');
      }
    }

    edit(){
      this.isEdited = true;
      this.notification = false;
      this.postUpdateOrCreate = false;
      this.enableFields();
    }

    exitdit(){
      this.isEdited = false;
      this.notification = false;
      this.postUpdateOrCreate = false;
      this.populateFields();
      this.disableFields();
    }

    exitcreation(){
      this.isCreation = false;
      this.isEdited = false;
      this.notification = false;
      this.postUpdateOrCreate = false;
      this.populateFields();
      this.disableFields();
    }

    closeNotification(){
      this.notification = false;
      this.isCreation = false;
      this.isEdited = false;
      this.postUpdateOrCreate = false;
      this.isDeletion = false;
      this.postUpdateOrCreate = false;
    }

    update(){
      if (this.buildingForm.valid) {

        const updateBuilding = {
          Name : this.buildingForm.controls['nombreEdificio'].value,
          Floors : this.buildingForm.controls['pisosEdificio'].value,
          communityId : this.Edificio.communityId
        };
         this.service.update(this.Edificio.id, updateBuilding).subscribe({
          next: (response) => {
            if (response.status === 200) {
              this.postUpdateOrCreate = true;
              setTimeout(() => {
                this.isEdited = false;
                this.postUpdateOrCreate = false;
                this.ngOnInit();
              }, 3000);
            }
          },
          error: (error) => {
            console.log('Error:', error);
          }
          });
      }
    }

    create(){
      this.isCreation = true;
      this.notification = false;
      this.postUpdateOrCreate = false;
      this.cleanFields();
    }

    saveCreate(){
      if (this.buildingForm.valid) {
        const createBuilding = {
          Name : this.buildingForm.controls['nombreEdificio'].value,
          Floors : this.buildingForm.controls['pisosEdificio'].value,
          communityId : this.Edificio.communityId
        };
        console.log(createBuilding); 

        this.service.create(createBuilding).subscribe({
          next: (response) => {
            if (response.status === 204) {
              this.postUpdateOrCreate = true;
              setTimeout(() => {
                this.isCreation = false;
                this.postUpdateOrCreate = false;
                this.ngOnInit();
              }, 3000);
            }
          },
          error: (error) => {
            console.log('Error:', error);
          }
          });

      }
    }

    delete(){
      if (this.Edificio.units >= 1){
        this.notification = true;
      } else {
        this.service.delete(this.Edificio.id).subscribe({
          next: (response) => {
            if (response.status === 200){
              this.postUpdateOrCreate = true;
              this.isDeletion = true;
              setTimeout(() => {
                this.isCreation = false;
                this.isDeletion = false;
                this.postUpdateOrCreate = false;
                this.ngOnInit();
              }, 3000);
            }
          },
          error: (error) => {
            console.log('Error:', error);
          }
        });
      }
    }
}