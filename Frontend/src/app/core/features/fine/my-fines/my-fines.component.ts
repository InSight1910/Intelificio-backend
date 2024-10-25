import {Component, OnInit} from '@angular/core';
import {AddAssignedFineComponent} from "../add-assigned-fine/add-assigned-fine.component";
import {AddFineComponent} from "../add-fine/add-fine.component";
import {EditAssignedFineComponent} from "../edit-assigned-fine/edit-assigned-fine.component";
import {EditFineComponent} from "../edit-fine/edit-fine.component";
import {AssignFineData, Fine, FineDenomination} from "../../../../shared/models/fine.model";
import {FineService} from "../../../services/fine/fine.service";
import {Store} from "@ngrx/store";
import {AppState} from "../../../../states/intelificio.state";
import {selectCommunity} from "../../../../states/community/community.selectors";
import {tap} from "rxjs";
import {selectUser} from "../../../../states/auth/auth.selectors";
import {FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {NgClass, NgForOf, NgIf} from "@angular/common";

@Component({
  selector: 'app-my-fines',
  standalone: true,
  imports: [
    AddAssignedFineComponent,
    AddFineComponent,
    EditAssignedFineComponent,
    EditFineComponent,
    FormsModule,
    NgForOf,
    NgIf,
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './my-fines.component.html',
  styleUrl: './my-fines.component.css'
})
export class MyFinesComponent implements OnInit{
  assignedFines: AssignFineData[] = [];
  assignedFine!: AssignFineData;
  loading = false;
  CommunityID: number = 0;
  UserId: number = 0;
  isDetailModal: boolean = false;
  finesDenomination = FineDenomination;
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private service: FineService,
    private store: Store<AppState>
  ) {
    this.form = this.fb.group({
      fineName: new FormControl(''),
      EventDate: new FormControl(''),
      UserName: new FormControl(''),
      UnitName: new FormControl(''),
      Comment: new FormControl(''),
    });
  }

  ngOnInit(): void {
    this.getFines();
  }

  getFines(){
    this.loading = true;
    this.store
      .select(selectUser)
      .pipe(
        tap((u) => {
          this.UserId = u?.sub ?? 0;
          if(this.UserId > 0){
            this.service.getAssignedFinesByUserId(this.UserId).subscribe(
              (response: {data : AssignFineData[]}) =>{
                this.assignedFines = response.data;
                this.loading = false;
              }
            )
          }
        })
      ).subscribe();
  }

  closeDetail() {
    this.isDetailModal = false;
    this.getFines();
  }

  OpenDetail(assignFineData : AssignFineData) {
    this.assignedFine = assignFineData;
    this.form.patchValue({fineName: this.assignedFine.fineName + " - " + this.assignedFine.fineAmount + " " + FineDenomination[this.assignedFine.fineStatus]})
    this.form.patchValue({UnitName: this.assignedFine.unitType+" "+ this.assignedFine.unitNumber+" - Piso "+this.assignedFine.unitFloor+", "+this.assignedFine.unitBuildingName})
    this.form.patchValue({EventDate: this.assignedFine.eventDate})
    this.form.patchValue({UserName: this.assignedFine.user})
    this.form.patchValue({Comment: this.assignedFine.comment})
    this.form.disable();
    this.isDetailModal = true;
  }

  protected readonly FineDenomination = FineDenomination;
}
