import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {Store} from "@ngrx/store";
import {AppState} from "../../../../states/intelificio.state";
import {CommunityService} from "../../../services/community/community.service";
import {LocationService} from "../../../services/location/location.service";
import {AdminCommunityComponent} from "../../community/adminCommunity/admin-community.component";
import {NgClass} from "@angular/common";
import {Contact} from "../../../../shared/models/contact.model";
import {ContactService} from "../../../services/contact/contact.service";
import {AuthActions} from "../../../../states/auth/auth.actions";
import {ContactListComponent} from "../contact-list.component";

@Component({
  selector: 'app-add-contact',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './add-contact.component.html',
  styleUrl: './add-contact.component.css'
})
export class AddContactComponent implements OnInit{
  IsLoading: boolean = false;
  notification: boolean = false;
  IsSuccess: boolean = false;
  IsError: boolean = false;
  notificationMessage: string = "";
  @Output() close = new EventEmitter<void>();
  @Input() CommunityID: number = 0;

  form: FormGroup;

  constructor(private fb: FormBuilder,
              private service: ContactService,
              private contactList: ContactListComponent) {
    this.form = this.fb.group({
      Name: new FormControl('', Validators.required),
      LastName: new FormControl('', Validators.required),
      Email: new FormControl('', Validators.required),
      PhoneNumber: new FormControl('', Validators.required),
      Service: new FormControl('', Validators.required),
      CommunityID: new FormControl(0),
    });
  }

  ngOnInit() {}

  onClose() {
    this.close.emit();
  }

  OnSubmit(){
    this.IsLoading = true;
    if (this.form.valid){
      const createContact  = {
        Name: this.form.value.Name,
        LastName:this.form.value.LastName,
        Email: this.form.value.Email,
        PhoneNumber: this.form.value.PhoneNumber,
        Service:  this.form.value.Service,
        CommunityID: this.CommunityID,
      };

      this.service.create(createContact).subscribe(
        {
          next: (response) => {
            if (response.status === 204) {
              this.IsLoading = false;
              this.notificationMessage =
                'Datos actualizados correctamente';
              this.IsSuccess = true;
              this.notification = true;
              setTimeout(() => {
                this.notification = false;
                this.IsSuccess = false;
                this.IsLoading = false;
                this.contactList.getContacts();
                this.close.emit();
              }, 5000);
            }
          },
          error: (error) =>{
            this.IsLoading = false;
            this.notificationMessage = 'No fue posible guardar este contacto';
            this.IsError = true;
            this.notification = true;
            setTimeout(() => {
              this.notification = false;
              this.notificationMessage = '';
              this.IsError = false;
            }, 5000);
          }
        }
      );
    }
  }

  closeNotification(){
    this.notification = false;
    this.IsSuccess = false;
    this.IsError = false;
  }
}
