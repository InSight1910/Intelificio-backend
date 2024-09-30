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
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Response } from '../../../../shared/models/response.model';

@Component({
  selector: 'app-manage',
  standalone: true,
  imports: [ModalComponent, ReactiveFormsModule, CommonModule],
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
      name: [''],
      location: [''],
      capacity: [''],
      isInMaintenance: [''],
      id: [''],
    });
  }
  form: FormGroup;
  isModalOpen: boolean = false;
  modalTitle: string = '';

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
    this.commonSpaceService.getCommonSpace(id).subscribe((response) => {
      this.form.patchValue(response.data);
    });
  }

  onClickEdit(id: number) {
    this.isModalOpen = true;
    this.modalTitle = 'Editar espacio común';
    this.loadCommonSpace(id);
  }

  onClickSaveEdit() {
    const commonSpaceId = this.form.get('id')?.value;
    const commonSpace: UpdateCommonSpace = {
      capacity: this.form.get('capacity')?.value,
      location: this.form.get('location')?.value,
      name: this.form.get('name')?.value,
      isInMaintenance: this.form.get('isInMaintenance')?.value,
    };
    this.commonSpaceService
      .updateCommonSpace(commonSpaceId, commonSpace)
      .subscribe({
        next: ({ data }) => {
          const commonSpacesList: CommonSpace[] =
            this.commonSpacesSubject.getValue();
          const index = commonSpacesList.findIndex(
            (c) => c.id === commonSpaceId
          );
          if (index !== -1) {
            commonSpacesList[index] = data;
            this.commonSpacesSubject.next(commonSpacesList);
          }
          this.isModalOpen = false;
        },
        error: (error) => {},
      });
  }

  onClickCloseEdit() {
    this.isModalOpen = false;
  }

  onClickAdd() {
    this.modalTitle = 'Agregar espacio común';
    this.isModalOpen = true;
  }

  onClickSaveAdd() {
    const commonSpace: CreateCommonSpace = {
      capacity: this.form.get('capacity')?.value,
      location: this.form.get('location')?.value,
      name: this.form.get('name')?.value,
      isInMaintenance: this.form.get('isInMaintenance')?.value,
      communityId: this.communityId,
    };
    this.commonSpaceService.createCommonSpace(commonSpace).subscribe({
      next: ({ data }) => {
        const commonSpacesList: CommonSpace[] =
          this.commonSpacesSubject.getValue();
        commonSpacesList.push(data);
        this.commonSpacesSubject.next(commonSpacesList);
        this.isModalOpen = false;
      },
      error: (error) => {},
    });
  }
}
