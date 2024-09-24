import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../../../services/auth/auth.service';
import { catchError, tap, of, timeout, Observable } from 'rxjs';
import { CommunityService } from '../../../../../services/community/community.service';
import { Community } from '../../../../../../shared/models/community.model';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../../../states/intelificio.state';
import { selectCommunity } from '../../../../../../states/community/community.selectors';

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
  canSearch: boolean = true;

  isSearching: boolean = false;
  isAdding: boolean = false;

  selectedFile: File | null = null;
  selectedFileName: string = '';
  isFileUploaded: boolean = false;
  community!: Observable<Community | null>;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private communityService: CommunityService,
    private store: Store<AppState>
  ) {
    this.form = this.fb.group({
      id: [''],
      email: [''],
      name: [{ value: '', disabled: true }],
      phoneNumber: [{ value: '', disabled: true }],
      role: [{ value: '', disabled: true }],
    });
    this.community = this.store.select(selectCommunity);
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
    if (this.isFileUploaded) {
      const formData = new FormData();
      formData.append('file', this.selectedFile!);

      this.communityService
        .addUserToCommunityWithFile(formData)
        .pipe(
          tap(() => {
            this.isSuccess = true;
            this.isAdding = false;
            setTimeout(() => {
              this.isSuccess = false;
              this.form.reset();
              this.canSearch = true;
              this.selectedFile = null;
              this.selectedFileName = '';
              this.isFileUploaded = false;
            }, 2000);
            this.addUserEvent.emit(true);
          })
        )
        .subscribe();
    } else {
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
            }, 2000);
            this.form.reset();
            this.addUserEvent.emit(true);
          }),
          catchError((error) => {
            this.canAddUser = false;
            this.isAdding = false;
            this.isSuccess = false;
            this.errors = error.error;
            return of(error);
          })
        )
        .subscribe();
    }
  }
  preventScroll(event: Event) {
    event.preventDefault();
    event.stopPropagation();
    return false;
  }

  onClickOpenModal() {
    this.isOpen = true;
    document.body.style.overflow = 'hidden';
    document.documentElement.style.overflow = 'hidden';
    window.addEventListener('scroll', this.preventScroll, { passive: false });
  }
  onClickCloseModal() {
    this.isOpen = false;
    this.errors = null;
    this.form.reset();

    document.body.style.overflowY = 'clip';
    document.documentElement.style.overflowY = 'scroll';
    window.removeEventListener('scroll', this.preventScroll);
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    console.log(input.files);
    if (input.files?.length) {
      this.form.disable();
      this.form.reset();

      this.selectedFile = input.files![0];
      this.selectedFileName = input.files![0].name;
      this.isFileUploaded = true;
      this.canAddUser = true;
      this.canSearch = false;
    }
  }
}
