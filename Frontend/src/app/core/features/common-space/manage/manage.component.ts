import { Component } from '@angular/core';
import {
  CommonSpace,
  CreateCommonSpace,
  UpdateCommonSpace,
} from '../../../../shared/models/commonspace.model';
import { CommonSpaceService } from '../../../services/commonspace/commonspace.service';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../states/intelificio.state';
import { selectCommunity } from '../../../../states/community/community.selectors';
import { tap, Observable, of, from, BehaviorSubject } from 'rxjs';
import { ModalComponent } from '../modal/modal.component';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MessageComponent } from '../../../../shared/component/error/message.component';

@Component({
  selector: 'app-manage',
  standalone: true,
  imports: [
    ModalComponent,
    ReactiveFormsModule,
    CommonModule,
    MessageComponent,
  ],
  templateUrl: './manage.component.html',
  styleUrl: './manage.component.css',
})
export class ManageComponent {
  constructor(
    private commonSpaceService: CommonSpaceService,
    private fb: FormBuilder,
    private store: Store<AppState>
  ) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      location: ['', Validators.required],
      capacity: [0, Validators.required],
      isInMaintenance: [false],
      id: [''],
    });
  }
  form: FormGroup;
  isModalOpen: boolean = false;
  isModalDeleteOpen: boolean = false;
  isCreating: boolean = false;
  isLoading: boolean = false;
  isSaving: boolean = false;
  modalTitle: string = '';
  modalButtonTitle: string = '';
  errors: { message: string }[] = [];
  successMessage: string = '';

  communityId: number = 1;
  private commonSpacesSubject = new BehaviorSubject<CommonSpace[]>([]);
  commonSpaces$: Observable<CommonSpace[]> =
    this.commonSpacesSubject.asObservable();

  ngOnInit() {
    this.loadCommonSpaces();
  }

  loadCommonSpaces() {
    this.store
      .select(selectCommunity)
      .pipe(
        tap((community) => {
          if (community) {
            this.communityId = community.id!;
            this.commonSpaceService
              .getCommonSpacesByCommunity(community!.id!)
              .subscribe(({ data }) => {
                this.commonSpacesSubject.next(data);
              });
          }
        })
      )
      .subscribe();
  }

  loadCommonSpace(id: number) {
    this.isLoading = true;
    this.commonSpaceService.getCommonSpace(id).subscribe((response) => {
      this.form.patchValue(response.data);
      this.isLoading = false;
    });
  }

  onClickEdit(id: number) {
    this.isModalOpen = true;
    this.modalTitle = 'Editar espacio común';
    this.isCreating = false;
    this.modalButtonTitle = 'Guardar';
    this.loadCommonSpace(id);
  }

  onClickSaveEdit() {
    const commonSpaceId = this.form.get('id')?.value;
    const commonSpace: UpdateCommonSpace = {
      capacity: this.form.get('capacity')?.value,
      location: this.form.get('location')?.value,
      name: this.form.get('name')?.value,
      IsInMaintenance: this.form.get('isInMaintenance')?.value,
    };
    this.successMessage = '';
    this.errors = [];
    this.isLoading = true;
    this.isSaving = true;
    this.commonSpaceService
      .updateCommonSpace(commonSpaceId, commonSpace)
      .subscribe({
        next: ({ data }) => {
          this.isLoading = false;
          this.isSaving = false;
          const commonSpacesList: CommonSpace[] =
            this.commonSpacesSubject.getValue();
          const index = commonSpacesList.findIndex(
            (c) => c.id === commonSpaceId
          );
          if (index !== -1) {
            commonSpacesList[index] = data;
            this.commonSpacesSubject.next(commonSpacesList);
          }
          this.successMessage = 'Espacio común guardado correctamente';
          setTimeout(() => {
            this.successMessage = '';
            this.isModalOpen = false;
            this.form.reset();
          }, 3000);
        },
        error: ({ error }) => {
          this.isLoading = false;
          this.isSaving = false;
          this.form.setErrors({ invalid: true });
          setTimeout(() => {
            this.errors = error;
          }, 3000);
        },
      });
  }

  onClickCloseEdit() {
    this.isModalOpen = false;
    this.form.reset();
  }

  onClickAdd() {
    this.modalTitle = 'Agregar espacio común';
    this.isModalOpen = true;
    this.isCreating = true;
    this.modalButtonTitle = 'Agregar';
  }

  onClickSaveAdd() {
    const commonSpace: CreateCommonSpace = {
      capacity: this.form.get('capacity')?.value,
      location: this.form.get('location')?.value,
      name: this.form.get('name')?.value,
      isInMaintenance: this.form.get('isInMaintenance')?.value,
      communityId: this.communityId,
    };
    this.isLoading = true;
    this.form.disable();
    this.commonSpaceService.createCommonSpace(commonSpace).subscribe({
      next: ({ data }) => {
        this.isLoading = false;
        const commonSpacesList: CommonSpace[] =
          this.commonSpacesSubject.getValue();
        commonSpacesList.push(data);
        this.commonSpacesSubject.next(commonSpacesList);
        this.successMessage = 'Espacio común agregado correctamente';
        setTimeout(() => {
          this.successMessage = '';
          this.isModalOpen = false;
        }, 3000);
      },
      error: ({ error }) => {
        this.isLoading;
        this.form.setErrors({ invalid: true });
        this.errors = error;
      },
    });
  }
  onClickClose() {
    this.isModalOpen = false;
    this.isCreating = false;
    this.modalTitle = '';
    this.modalButtonTitle = '';
    this.errors = [];
    this.successMessage = '';
    this.form.reset();
  }

  onSubmit(event: Event | void) {
    if (event) event.preventDefault();
    if (this.form.valid) {
      if (this.isCreating) {
        this.onClickSaveAdd();
      } else {
        this.onClickSaveEdit();
      }
    }
  }

  commonSpaceToDelete: number = 0;
  canDelete: boolean = true;
  onClickDelete(id: number) {
    this.isModalDeleteOpen = true;
    this.commonSpaceToDelete = id;
    this.canDelete = true;
  }

  onClickCloseDelete() {
    this.isModalDeleteOpen = false;
    this.commonSpaceToDelete = 0;
  }

  onDelete() {
    this.errors = [];
    this.successMessage = '';
    this.isLoading = true;
    if (this.commonSpaceToDelete > 0) {
      this.commonSpaceService
        .deleteCommonSpace(this.commonSpaceToDelete)
        .subscribe({
          next: () => {
            this.isLoading = false;
            const commonSpacesList: CommonSpace[] =
              this.commonSpacesSubject.getValue();
            const index = commonSpacesList.findIndex(
              (c) => c.id === this.commonSpaceToDelete
            );
            if (index !== -1) {
              commonSpacesList.splice(index, 1);
              this.commonSpacesSubject.next(commonSpacesList);
            }
            this.successMessage = 'Espacio común eliminado correctamente';
            this.canDelete = false;
            setTimeout(() => {
              this.successMessage = '';
              this.isModalDeleteOpen = false;
            }, 3000);
          },
          error: ({ error }) => {
            this.isLoading = false;
            this.errors = error;
            this.canDelete = false;
            setTimeout(() => {
              this.errors = [];
              this.isModalDeleteOpen;
            }, 3000);
          },
        });
    }
  }
}
