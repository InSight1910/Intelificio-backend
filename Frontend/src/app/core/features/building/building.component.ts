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
    
    Edificios: Building[] = [];
    selectedBuildingId: number = 1;
    Edificio: Building = {
      id: 0, 
      name: 'Edificio de prueba', 
      floors: 1, 
      units: 10, 
      CommunityID: 1, 
      communityName: 'Comunidad de prueba' 
    };
    
    buildingForm = new FormGroup({
      nombreEdificio: new FormControl(''),
      pisosEdificio: new FormControl(0),
      ComunidadEdificio: new FormControl(''),
      CantidadUnidades: new FormControl(0)
    });

    isEdited = false;
    
    constructor(
      private service: BuildingService
    ){}

    ngOnInit(): void {
        this.getNumberOfBuilding();
    }

    async getNumberOfBuilding(){
      this.service.getbyCommunityId(2).subscribe(
        (response: {data: Building[]}) => {
            this.Edificios = response.data;
            if (this.Edificios.length > 0) {
              // Seleccionar el primer edificio por defecto
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
      const selectedBuilding = this.Edificios?.find(building => building.id === id);
  
      if (selectedBuilding) {
        this.Edificio = selectedBuilding;
        this.populateUserData();
        this.disableFields();
      } else {
        console.error('Edificio no encontrado');
      }
    }

    private populateUserData(): void {
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
      this.buildingForm.controls['CantidadUnidades'].enable();
      this.buildingForm.controls['ComunidadEdificio'].enable();
    }

    edit(){
      this.isEdited = true;
      this.enableFields();
    }

    exitdit(){
      this.isEdited = false;
      this.populateUserData();
      this.disableFields();
    }
}