import {Component, OnInit} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {AssignedFine, AssignFineData, Fine, FineDenomination} from "../../../shared/models/fine.model";
import {NgClass, NgForOf, NgIf, TitleCasePipe} from "@angular/common";
import {AddContactComponent} from "../contact-list/add-contact/add-contact.component";
import {AddFineComponent} from "./add-fine/add-fine.component";
import {selectCommunity} from "../../../states/community/community.selectors";
import {tap} from "rxjs";
import {Store} from "@ngrx/store";
import {AppState} from "../../../states/intelificio.state";
import {FineService} from "../../services/fine/fine.service";
import {EditFineComponent} from "./edit-fine/edit-fine.component";
import {EditAssignedFineComponent} from "./edit-assigned-fine/edit-assigned-fine.component";
import {AddAssignedFineComponent} from "./add-assigned-fine/add-assigned-fine.component";


@Component({
  selector: 'app-fine',
  standalone: true,
  imports: [
    FormsModule,
    NgForOf,
    NgIf,
    AddContactComponent,
    AddFineComponent,
    TitleCasePipe,
    EditFineComponent,
    EditAssignedFineComponent,
    AddAssignedFineComponent,
    NgClass,
    ReactiveFormsModule
  ],
  templateUrl: './fine.component.html',
  styleUrl: './fine.component.css'
})
export class FineComponent implements OnInit{

  isOpenModalAddFine: boolean = false;
  isOpenModalEditFine: boolean = false;
  isOpenModalEditAssigmentFine: boolean = false;
  isOpenModalAddAssigmentFine: boolean = false;
  isOpenModalDeleteFine: boolean = false;
  isConfirmCancelModal: boolean = false;
  notification: boolean = false;
  IsSuccess: boolean = false;
  IsError: boolean = false;
  notificationMessage: string = '';
  CommunityID: number = 0;
  fine!: Fine;
  finesDenomination = FineDenomination;
  fines: Fine[] = [];
  assignedFines: AssignFineData[] = [];
  assignedFine!: AssignFineData;
  selectedTab: string = 'existing';
  loading = false;
  edit: boolean = false;


  constructor(
    private service: FineService,
    private store: Store<AppState>
  ) {}

  ngOnInit(): void {
    this.loading = true;
    this.store
      .select(selectCommunity)
      .pipe(
        tap((c) => {
          this.CommunityID = c?.id ?? 0;
          if(this.CommunityID > 0){
            this.service.getAllFinesbyCommunityId(this.CommunityID).subscribe(
              (response: {data : Fine[]}) =>{
                this.fines = response.data;
                this.loading = false;
              }
            )
            this.getAssignedFines();
          }
        })
      ).subscribe();
  }

  openAddFineModal() {
    this.isOpenModalAddFine = true;
  }
  closeAddFineModal() {
    this.isOpenModalAddFine = false;
    this.getFines();
  }
  opendEditFineModal(fine: Fine): void {
    this.fine = fine;
    this.isOpenModalEditFine = true;
  }
  closeEditFineModal() {
    this.isOpenModalEditFine = false;
    this.getFines();
  }
  openEditAssignedFineModal(edit: boolean, assignedFine: AssignFineData) {
    this.edit = edit;
    this.assignedFine = assignedFine;
    this.isOpenModalEditAssigmentFine = true;
  }
  closeEditAssignedFineModal() {
    this.isOpenModalEditAssigmentFine = false;
    this.getAssignedFines();
  }

  openAddAssignedFineModal() {
    this.isOpenModalAddAssigmentFine = true;
  }
  closeAddAssignedFineModal() {
    this.isOpenModalAddAssigmentFine = false;
    this.getAssignedFines();
  }

  closeDeleteFineModal(){
    this.isOpenModalDeleteFine = false;
    this.notification = false;
    this.notificationMessage = "";
    this.getFines();
  }

  closeCancelModal(){
    this.isConfirmCancelModal = false;
    this.getAssignedFines();
  }

  selectTab(tab: string): void {
    this.selectedTab = tab;
  }



  getFines(){
    this.service.getAllFinesbyCommunityId(this.CommunityID).subscribe(
      (response: {data : Fine[]}) =>{
        this.fines = response.data;
      }
    )
  }

  getAssignedFines(){
    this.service.getAllAssignedFinesbyCommunityId(this.CommunityID).subscribe(
      (response: {data : AssignFineData[]}) =>{
        this.assignedFines = response.data;
      }
    )
  }


  delete(fineId : number){
    this.service.deleteFine(fineId).subscribe({
      next: (response) => {
        if (response.status === 200) {
          this.notificationMessage = 'Multa eliminada satisfactoriamente.';
          this.IsSuccess = true;
          this.notification = true;
          this.isOpenModalDeleteFine = true;
          setTimeout(() => {
            this.notification = false;
            this.IsSuccess = false;
            this.isOpenModalDeleteFine = false;
          }, 2000);
        }
      },
      error: (error) => {
        if (error.status === 400) {
          const errorData = error.error?.[0];
          this.notificationMessage = errorData.message;
          this.IsError = true;
          this.notification = true;
          this.isOpenModalDeleteFine = true;
          setTimeout(() => {
            this.notification = false;
            this.notificationMessage = '';
            this.IsError = false;
            this.isOpenModalDeleteFine = false;
          }, 5000);
        } else {
          this.notificationMessage = 'No fue posible eliminar esta multa';
          this.IsError = true;
          this.notification = true;
          setTimeout(() => {
            this.notification = false;
            this.notificationMessage = '';
            this.IsError = false;
            this.isOpenModalDeleteFine = false;
          }, 4000);
        }
      },
    })
  }

  openCancel(assignedFine: AssignFineData) {
    this.isConfirmCancelModal = true;
    this.assignedFine = assignedFine;
  }

  cancel(id: number){
    this.loading = true;
    this.service.deleteAssignedFine(id).subscribe({
      next: (response) => {
        if (response.status === 200) {
          this.notificationMessage = 'Multa condonada satisfactoriamente.';
          this.IsSuccess = true;
          this.notification = true;
          this.loading = false;
          setTimeout(() => {
            this.notification = false;
            this.IsSuccess = false;
            this.closeCancelModal();
          }, 2000);
        }
      },
      error: (error) => {
        if (error.status === 400) {
          const errorData = error.error?.[0];
          this.notificationMessage = errorData.message;
          this.IsError = true;
          this.notification = true;
          this.loading = false;
          setTimeout(() => {
            this.notification = false;
            this.notificationMessage = '';
            this.IsError = false;
            this.closeCancelModal();
          }, 5000);
        } else {
          this.notificationMessage = 'No fue posible condonar esta multa';
          this.IsError = true;
          this.notification = true;
          this.loading = false;
          setTimeout(() => {
            this.notification = false;
            this.notificationMessage = '';
            this.IsError = false;
            this.closeCancelModal();
          }, 4000);
        }
      },
    })
  }


}
