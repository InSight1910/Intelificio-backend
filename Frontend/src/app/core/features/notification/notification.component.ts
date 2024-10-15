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
import {Router} from "@angular/router";
import {PrintNotificationComponent} from "./print-notification/print-notification.component";
type RecipientLevel = 'community' | 'building' | 'floor';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, PrintNotificationComponent],
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
  showPrintModal = false;
  notificationData: any = {};

  recipientLevel: RecipientLevel = 'community';
  notification = false;
  IsSuccess = false;
  IsError = false;
  notificationMessage = '';
  maxMessageLength = 1064;
  messageLength = 0;

  constructor(
    private fb: FormBuilder,
    private EmailService: NotificationService,
    private BuildingService: BuildingService,
    private store: Store<AppState>,
    private router: Router
  ) {
    this.emailForm = this.fb.group({
      Subject: ['', Validators.required],
      Title: ['', Validators.required],
      Message: ['', [Validators.required,Validators.maxLength(this.maxMessageLength)]],
      SenderName: ['', Validators.required],
      ComunityID: [this.community?.id, Validators.required],
      BuildingID: [0],
      Floor: [0],
    });
    this.updateCharacterCount();
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

  updateCharacterCount() {
    const messageControl = this.emailForm.get('Message');
    if (messageControl) {
      this.messageLength = messageControl.value ? messageControl.value.length : 0;
    }
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
              this.emailForm.controls['ComunityID'].setValue(this.community?.id);
            }, 5000);
          } else {
            this.isLoading = false;
            this.notificationMessage = 'No fue posible enviar el Comunicado.';
            this.IsError = true;
            this.notification = true;
            setTimeout(() => {
              this.notification = false;
              this.IsError = false;
              this.emailForm.reset();
              this.emailForm.controls['ComunityID'].setValue(this.community?.id);
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
            this.emailForm.controls['ComunityID'].setValue(this.community?.id);
          }, 5000);
          console.log('Error:', error.error);
        },
      });
    }
  }

  clean(){
    this.emailForm.reset();
    this.emailForm.controls['ComunityID'].setValue(this.community?.id);
    this.notification = false;
    this.IsSuccess = false;
    this.IsError = false;
  }

  onRecipientLevelChange(event: Event) {
    const selectedValue = (event.target as HTMLSelectElement).value;
    this.recipientLevel = selectedValue as RecipientLevel;
    this.getNumberOfBuilding();
    this.floors = [];

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

  openPrintModal() {
    this.notificationData = {
      SenderName: this.emailForm.get('SenderName')?.value,
      CommunityName: this.community?.name,
      Title: this.emailForm.get('Title')?.value,
      Message: this.emailForm.get('Message')?.value,
    };
    this.showPrintModal = true;
  }

  // Close the modal
  closePrintModal() {
    this.showPrintModal = false;
  }

  // Trigger the print functionality
  printContent() {
    const printContents = document.querySelector('.print-content')?.innerHTML;
    const iframe = document.createElement('iframe');

    //iframe.style.position = 'absolute';
    //iframe.style.width = '0';
    //iframe.style.height = '0';
    //iframe.style.border = 'none';

    document.body.appendChild(iframe);

    const doc = iframe.contentWindow?.document;
    if (doc && printContents) {
      doc.open();
      doc.write(`
      <html>
        <head>
          <title>Imprimir Comunicado</title>
          <link
      rel="stylesheet"
      href="https://cdn.jsdelivr.net/npm/bulma@1.0.2/css/bulma.min.css"
    />
        </head>
        <body>
          ${printContents}
        </body>
      </html>
    `);
      doc.close();

      iframe.contentWindow?.focus();
      iframe.contentWindow?.print();
    }

    iframe.onload = () => document.body.removeChild(iframe);
  }
}
