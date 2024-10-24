import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors, ValidatorFn,
  Validators
} from "@angular/forms";
import {CreateAssignedFine, Fine, FineDenomination} from "../../../../shared/models/fine.model";
import {FineService} from "../../../services/fine/fine.service";
import {Building} from "../../../../shared/models/building.model";
import {Unit} from "../../../../shared/models/unit.model";
import {selectCommunity} from "../../../../states/community/community.selectors";
import {tap} from "rxjs";
import {Store} from "@ngrx/store";
import {AppState} from "../../../../states/intelificio.state";
import {BuildingService} from "../../../services/building/building.service";
import {UnitService} from "../../../services/unit/unit.service";

@Component({
  selector: 'app-add-assigned-fine',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './add-assigned-fine.component.html',
  styleUrl: './add-assigned-fine.component.css'
})
export class AddAssignedFineComponent implements OnInit {
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
  @Input() fines!: Fine[];
  @Input() CommunityId: number = 0;

  constructor(private fb: FormBuilder,
              private service: FineService,
              private buildingService: BuildingService,
              private unitService: UnitService,) {
    this.form = this.fb.group({
      EventDate: new FormControl('',[Validators.required,this.eventDateValidator()]),
      UserName: new FormControl(''),
      CommunityID: new FormControl(0,Validators.required),
      BuildingID: new FormControl(0,Validators.required),
      UnitID: new FormControl(0,Validators.required),
      Comment: new FormControl('',Validators.required),
      FineId: new FormControl(0,Validators.required),
    });
  }

  ngOnInit(): void {
    this.form.controls['UserName'].disable();
    this.getNumberOfBuilding();
  }

  getNumberOfBuilding() {
    this.IsLoading = true;
    this.buildingService.getbyCommunityId(this.CommunityId).subscribe(
      (response: { data: Building[] }) => {
        this.buildings = response.data;
        if (this.buildings.length > 0) {
          this.form.patchValue({ CommunityID: this.CommunityId });
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
      const createAssignedFine : CreateAssignedFine = {
        fineId : this.fine.fineId,
        unitId : this.form.get('UnitID')?.value,
        eventDate : this.form.get('EventDate')?.value,
        comment: this.form.get('Comment')?.value,
      };
      console.log(createAssignedFine);
      this.service.createAssignedFine(createAssignedFine).subscribe({
        next: (response) => {
          if (response.status === 204) {
            this.IsLoading = false;
            this.notificationMessage = 'Multa asignada correctamente.';
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
            }, 5000);
          } else {
            this.notificationMessage = 'No fue posible asignar la multa.';
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

  protected readonly FineDenomination = FineDenomination;
}
