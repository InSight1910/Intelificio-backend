import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule, ValidationErrors,
  ValidatorFn,
  Validators
} from "@angular/forms";
import {
  AssignedFine,
  AssignFineData,
  CreateAssignedFine,
  Fine,
  FineDenomination, UpdateAssignedFine
} from "../../../../shared/models/fine.model";
import {FineService} from "../../../services/fine/fine.service";
import {Building} from "../../../../shared/models/building.model";
import {Unit} from "../../../../shared/models/unit.model";
import {BuildingService} from "../../../services/building/building.service";
import {UnitService} from "../../../services/unit/unit.service";

@Component({
  selector: 'app-edit-assigned-fine',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './edit-assigned-fine.component.html',
  styleUrl: './edit-assigned-fine.component.css'
})
export class EditAssignedFineComponent implements OnInit {

  IsLoading: boolean = false;
  notification: boolean = false;
  IsSuccess: boolean = false;
  IsError: boolean = false;
  notificationMessage: string = '';
  maxMessageLength = 150;
  messageLength = 0;
  form: FormGroup;
  denominations: { value: FineDenomination; key: number }[] = [];

  buildings: Building[] = [];
  building!: Building;
  units: Unit[] = [];
  fine!: Fine;

  @Output() close = new EventEmitter<void>();
  @Input() assignedFine!: AssignFineData;
  @Input() edit: boolean = false;
  @Input() CommunityId: number = 0;
  @Input() fines!: Fine[];

  constructor(private fb: FormBuilder,
              private service: FineService,
              private buildingService: BuildingService,
              private unitService: UnitService,) {
    this.form = this.fb.group({
      EventDate: new FormControl('',[Validators.required,this.eventDateValidator()]),
      UserName: new FormControl('',Validators.required),
      CommunityID: new FormControl(0,Validators.required),
      BuildingID: new FormControl(0,Validators.required),
      UnitID: new FormControl(0,Validators.required),
      Comment: new FormControl('',Validators.required),
      FineId: new FormControl(0,Validators.required),
      FineName: new FormControl(''),
    });
  }

  ngOnInit(): void {
    this.getNumberOfBuilding();
    if(this.edit){}else {this.form.disable();}
  }

  getNumberOfBuilding() {
    this.IsLoading = true;
    this.buildingService.getbyCommunityId(this.CommunityId).subscribe(
      (response: { data: Building[] }) => {
        this.buildings = response.data;
        if (this.buildings.length > 0) {
          if(!this.edit){
            this.form.patchValue({ CommunityID: this.CommunityId });
            this.form.patchValue({FineName: this.assignedFine.fineName + " - " + this.assignedFine.fineAmount + " " + FineDenomination[this.assignedFine.fineStatus]})
            this.form.patchValue({BuildingID: this.assignedFine.unitBuildingId})
            this.onChangeBuilding();
            this.form.patchValue({UnitID: this.assignedFine.unitId})
            this.form.patchValue({EventDate: this.assignedFine.eventDate})
            this.form.patchValue({UserName: this.assignedFine.user})
            this.form.patchValue({Comment: this.assignedFine.comment})
          }
          this.IsLoading = false;
        }
      },
      (error) => {
        this.IsLoading = false;
        console.error('Error al obtener edificios', error);
      }
    );
  }

  onChangeBuilding() {
    this.form.controls['UnitID'].setValue(0);
    if(this.form.get('BuildingID')?.value != 0){
      this.unitService.getUnitsByBuilding(this.form.get('BuildingID')?.value).subscribe(data => {
        this.units = data.data;
      })
    }
  }
  onChangeUnit() {
    const unit = this.units.find(
      (x) => x.id == this.form.get('UnitID')?.value
    )!;
    this.form.controls['UserName'].setValue(unit.user);
    this.form.controls['UnitID'].setValue(unit.id);
  }

  editMode() {
    this.form.patchValue({FineId: this.assignedFine.fineId});
    this.form.patchValue({BuildingID: this.assignedFine.unitBuildingId});
    this.onChangeBuilding();
    this.form.patchValue({UnitID: this.assignedFine.unitId})
    this.form.patchValue({EventDate: this.assignedFine.eventDate})
    this.form.patchValue({UserName: this.assignedFine.user})
    this.form.patchValue({Comment: this.assignedFine.comment})
    this.form.controls['Comment'].enable();
    this.edit = true;
  }

  onChangefine() {
    this.fine = this.fines.find(
      (x) => x.fineId == this.form.get('FineId')?.value
    )!;
  }


  eventDateValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const eventDate = new Date(control.value);
      const today = new Date();

      if (isNaN(eventDate.getTime())) {
        return { invalidDate: true };
      }

      return eventDate <= today ? null : { futureDate: true };
    };
  }


  updateCharacterCount() {
    const messageControl = this.form.get('Comment');
    if (messageControl) {
      this.messageLength = messageControl.value ? messageControl.value.length : 0;
    }
  }


  onInputChange(controlName: string): void {
    const control = this.form.get(controlName);
    if (control) {
      control.markAsUntouched();
    }
  }

  onClose() {
    this.close.emit();
  }

  OnSubmit(){
    this.IsLoading = true;
    if(this.form.valid
      && this.CommunityId > 0
      && this.form.get('UnitID')?.value > 0
    ){
      const [day, month, yearAndTime] = this.form.get('EventDate')?.value.split('-');
      const [year, time] = yearAndTime.split(' ');
      const formattedDateString = `${year}-${month}-${day}T${time}:00`;
      const localDate = new Date(formattedDateString);
      if (isNaN(localDate.getTime())) {
        console.error('Fecha inválida:', formattedDateString);
        this.IsLoading = false;
        return;
      }
      const utcDate = localDate.toISOString();

      const updateAssignedFine : UpdateAssignedFine = {
        assignedFineId : this.assignedFine.assignedFineID,
        fineId: this.form.get('FineId')?.value,
        unitId : this.form.get('UnitID')?.value,
        eventDate : utcDate,
        comment: this.form.get('Comment')?.value,
      };
      this.form.disable();
      this.service.updateAssignedFine(this.assignedFine.assignedFineID,updateAssignedFine).subscribe({
        next: (response) => {
          if (response.status === 200) {
            this.IsLoading = false;
            this.notificationMessage = 'Multa actualizada correctamente.';
            this.IsSuccess = true;
            this.notification = true;
            setTimeout(() => {
              this.form.enable();
              this.notification = false;
              this.IsSuccess = false;
              this.IsLoading = false;
              this.close.emit();
            }, 3000);
          }
        },
        error: error => {
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
              this.form.enable();
              this.close.emit();
            }, 5000);
          } else {
            this.notificationMessage = 'No fue posible actualizar la multa.';
            this.IsError = true;
            this.notification = true;
            setTimeout(() => {
              this.notification = false;
              this.notificationMessage = '';
              this.IsError = false;
              this.form.enable();
              this.close.emit();
            }, 5000);
          }
        },
      });

    }
  }



  protected readonly FineDenomination = FineDenomination;

}
