import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup,Validators, FormsModule,ReactiveFormsModule} from '@angular/forms';
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
      name: 'Edificio 1', 
      floors: 1, 
      units: 10, 
      communityId: 1, 
      communityName: 'Comunidad de prueba' 
    },
    {
      id: 2, 
      name: 'Edificio 2', 
      floors: 1, 
      units: 10, 
      communityId: 1, 
      communityName: 'Comunidad de prueba' 
    }];

    selectedBuildingId: number = 1;

    Edificio: Building = {
      id: 1, 
      name: 'Edificio de prueba', 
      floors: 1, 
      units: 10, 
      communityId: 1, 
      communityName: 'Comunidad de prueba' 
    };
    
    buildingForm = new FormGroup({
      nombreEdificio: new FormControl('', Validators.required),
      pisosEdificio: new FormControl(0, Validators.required),
      ComunidadEdificio: new FormControl('', Validators.required),
      CantidadUnidades: new FormControl(0, Validators.required)
    });

    isEdited = false;
    notification = false;
    isCreation = false;
    
    constructor(
      private service: BuildingService
    ){}

    ngOnInit(): void {
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

    detail(id: number){
      this.selectedBuildingId = id; 
      this.isEdited = false;
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


    edit(){
      this.isEdited = true;
      this.notification = false;
      this.enableFields();
    }

    exitdit(){
      this.isEdited = false;
      this.notification = false;
      this.populateFields();
      this.disableFields();
    }

    exitcreation(){
      this.isCreation = false;
      this.isEdited = false;
      this.notification = false;
      this.populateFields();
      this.disableFields();
    }

    delete(){
      if (this.Edificio.units >= 1){
        this.notification = true;
      } else {
        this.service.delete(this.Edificio.id).subscribe({
          next: (response) => {
            if (response.status === 200){
              this.ngOnInit();
            }
          },
          error: (error) => {
            console.log('Error:', error);
          }
        });
      }
    }

    closeNotification(){
      this.notification = false;
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
              this.ngOnInit();
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
              this.isCreation = false;
              this.ngOnInit();
            }
          },
          error: (error) => {
            console.log('Error:', error);
          }
          });

      }
    }


}