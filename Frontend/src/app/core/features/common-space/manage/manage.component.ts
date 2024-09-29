import { Component } from '@angular/core';
import { CommonSpace } from '../../../../shared/models/commonspace.model';
import { CommonSpaceService } from '../../../services/commonspace/commonspace.service';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../states/intelificio.state';
import { selectCommunity } from '../../../../states/community/community.selectors';
import { tap, Observable, of, from } from 'rxjs';
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
    });
  }
  form: FormGroup;
  isModalOpen: boolean = false;
  modalTitle: string = '';
  mockData: Response<CommonSpace[]> = {
    data: [
      {
        id: 1,
        name: 'Common Space 1',
        location: 'Location 1',
        capacity: 10,
        isInMaintenance: false,
        communityId: 1,
      },
      {
        id: 2,
        name: 'Common Space 2',
        location: 'Location 1',
        capacity: 10,
        isInMaintenance: true,
        communityId: 1,
      },
      {
        id: 3,
        name: 'Common Space 3',
        location: 'Location 1',
        capacity: 10,
        isInMaintenance: false,
        communityId: 1,
      },
    ],
  };
  commonSpaces$: Observable<Response<CommonSpace[]>> = of(this.mockData);

  ngOnInit() {
    this.loadCommonSpaces();
  }

  loadCommonSpaces() {
    this.store
      .select(selectCommunity)
      .pipe(
        tap((community) => {
          if (community) {
            this.commonSpaces$ =
              this.commonSpaceService.getCommonSpacesByCommunity(
                community!.id!
              );
          }
        })
      )
      .subscribe();
  }

  loadCommonSpace(id: number) {
    // this.commonSpaceService.getCommonSpace(id).subscribe((response) => {
    //   this.form.patchValue(response.data);
    // });

    this.commonSpaces$.subscribe((response) => {
      const space = response.data.find((x) => x.id === id);
      this.form.patchValue({
        name: space?.name,
        location: space?.location,
        capacity: space?.capacity,
        isInMaintenance: space?.isInMaintenance,
      });
    });
  }

  onClickEdit(id: number) {
    this.isModalOpen = true;
    this.modalTitle = 'Editar espacio común';
    this.loadCommonSpace(id);
  }

  onClickCloseEdit() {
    this.isModalOpen = false;
  }
}
