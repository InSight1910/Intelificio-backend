import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NotificationService } from '../../services/notification/notification.service';
import { Building } from '../../../shared/models/building.model';
import { BuildingService } from '../../services/building/building.service';
import { selectCommunity } from '../../../states/community/community.selectors';
import { Community } from '../../../shared/models/community.model';
import { AppState } from '../../../states/intelificio.state';
import { tap } from 'rxjs';
import { Store } from '@ngrx/store';
import { CommonModule } from '@angular/common';
import { Unit } from '../../../shared/models/unit.model';
import { UnitService } from '../../services/unit/unit.service';
type RecipientLevel = 'community' | 'building' | 'floor';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.css',
})
export class NotificationComponent implements OnInit {
  emailForm: FormGroup;
  buildings!: Building[];
  building!: Building;
  units: Unit[] = [];
  floors: number[] = [];
  community!: Community | null;
  isLoading = false;
  selectedBuildingId: number = 1;
  recipientLevel: RecipientLevel = 'community';
  notification = false;
  IsSuccess = false;
  IsError = false;
  notificationMessage = '';

  constructor(
    private fb: FormBuilder,
    private EmailService: NotificationService,
    private BuildingService: BuildingService,
    private store: Store<AppState>
  ) {
    this.emailForm = this.fb.group({
      Subject: ['', Validators.required],
      Title: ['', Validators.required],
      Message: ['', Validators.required],
      SenderName: ['', Validators.required],
      ComunityID: [this.community?.id, Validators.required],
      BuildingID: [0],
      Floor: [0],
    });
  }

  closeNotification() {
    this.notification = false;
    this.IsSuccess = false;
    this.IsError = false;
  }

  async ngOnInit() {
    await this.getNumberOfBuilding();
  }

  async getNumberOfBuilding() {
    this.store
      .select(selectCommunity)
      .pipe(
        tap((c) => {
          this.community = c;
          this.BuildingService.getbyCommunityId(c?.id!).subscribe(
            (response: { data: Building[] }) => {
              this.buildings = response.data;
              if (this.buildings.length > 0) {
                this.selectedBuildingId = this.buildings[0].id;
                this.emailForm.patchValue({ ComunityID: this.community?.id });
                this.isLoading = false;
              }
            },
            (error) => {
              this.isLoading = false;
              console.error('Error al obtener edificios', error);
            }
          );
        })
      )
      .subscribe();
  }

  onChangeBuilding() {
    const building = this.buildings.find(
      (x) => x.id == this.emailForm.get('BuildingID')?.value
    )!;
    this.floors = Array.from(
      { length: building.floors },
      (_, index) => index + 1
    );
  }

  onSubmit() {
    this.isLoading = true;
    if (this.emailForm.valid) {
      const email = {
        Subject: this.emailForm.controls['Subject'].value,
        Title: this.emailForm.controls['Title'].value,
        Message: this.emailForm.controls['Message'].value,
        SenderName: this.emailForm.controls['SenderName'].value,
        CommunityID: this.emailForm.controls['ComunityID'].value,
        BuildingID: this.emailForm.controls['BuildingID'].value,
        Floor: this.emailForm.controls['Floor'].value,
      };

      this.EmailService.SendEmail(email).subscribe({
        next: (response) => {
          if (response.status === 204) {
            this.isLoading = false;

            if (this.recipientLevel === 'community') {
              this.notificationMessage =
                'Comunicado interno enviado de forma exitosa a toda la comunidad.';
            } else if (this.recipientLevel === 'building') {
              this.notificationMessage =
                'Comunicado interno enviado de forma exitosa a todo el edificio.';
            } else {
              this.notificationMessage =
                'Comunicado interno enviado de forma exitosa a todo el piso.';
            }

            this.IsSuccess = true;
            this.notification = true;
            setTimeout(() => {
              this.notification = false;
              this.IsSuccess = false;
              this.emailForm.reset();
            }, 5000);
          } else {
            this.isLoading = false;
            this.notificationMessage = 'No fue posible enviar el Comunicado.';
            this.notification = true;
            this.IsError = true;
            setTimeout(() => {
              this.notification = false;
              this.IsError = false;
              this.emailForm.reset();
            }, 5000);
          }
        },
        error: (error) => {
          this.notificationMessage = 'No fue posible enviar el Comunicado.';
          this.notification = true;
          this.IsError = true;
          setTimeout(() => {
            this.notification = false;
            this.IsError = false;
            this.emailForm.reset();
          }, 5000);
          console.log('Error:', error.error);
        },
      });
    }
  }

  onRecipientLevelChange(event: Event) {
    const selectedValue = (event.target as HTMLSelectElement).value;
    this.recipientLevel = selectedValue as RecipientLevel;
    this.getNumberOfBuilding();

    if (this.recipientLevel === 'community') {
      this.emailForm.patchValue({
        ComunityID: this.community?.id,
        BuildingID: 0,
        Floor: 0,
      });
    } else if (this.recipientLevel === 'building') {
      this.emailForm.patchValue({
        Floor: 0,
      });
    }
  }
}
