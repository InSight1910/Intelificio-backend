import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../../../services/auth/auth.service';
import { catchError, tap, of, timeout } from 'rxjs';
import { CommunityService } from '../../../../../services/community/community.service';

@Component({
  selector: 'app-add-user-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-user-modal.component.html',
  styleUrl: './add-user-modal.component.css',
})
export class AddUserModalComponent {
  @Output() addUserEvent = new EventEmitter<boolean>();
  isOpen: boolean = false;
  form: FormGroup;
  errors!: { message: string }[] | null;
  isSuccess: boolean = false;
  canAddUser: boolean = false;

  isSearching: boolean = false;
  isAdding: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private communityService: CommunityService
  ) {
    this.form = this.fb.group({
      id: [''],
      email: [''],
      name: [{ value: '', disabled: true }],
      phoneNumber: [{ value: '', disabled: true }],
      role: [{ value: '', disabled: true }],
    });
  }

  onClickSearch() {
    this.errors = null;
    this.isSearching = true;
    const email = this.form.get('email')?.value;

    if (email == '' || email == null) {
      this.isSearching = false;
      this.errors = [{ message: 'Debe ingresar un email' }];
      return;
    }
    this.authService
      .getUserByEmail(email)
      .pipe(
        tap((response) => {
          this.form.patchValue({
            name: response.data.name,
            phoneNumber: response.data.phoneNumber,
            role: response.data.role,
            id: response.data.id,
          });
          this.canAddUser = true;
          this.isSearching = false;
        }),
        catchError((error) => {
          this.errors = error.error;
          this.form.reset();
          this.canAddUser = false;
          this.isSearching = false;
          return of(error);
        })
      )
      .subscribe();
  }

  onClickAddUser() {
    this.isAdding = true;
    const userID = this.form.get('id')?.value;
    const communityID = localStorage.getItem('communityId')!;
    console.log(userID, communityID);

    this.communityService
      .addUserToCommunity(+communityID, userID)
      .pipe(
        tap(() => {
          this.isSuccess = true;
          this.isAdding = false;
          setTimeout(() => {
            this.isSuccess = false;
            console.log('paso timeout');
          }, 2000);
          this.form.reset();
          this.addUserEvent.emit(true);
        }),
        catchError((error) => {
          this.canAddUser = false;
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
    this.form.reset();
  }
}
