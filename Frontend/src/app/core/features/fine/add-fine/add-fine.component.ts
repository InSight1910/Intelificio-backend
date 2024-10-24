import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {ContactService} from "../../../services/contact/contact.service";
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {FineService} from "../../../services/fine/fine.service";
import {CreateFine, Fine, FineDenomination} from "../../../../shared/models/fine.model";

@Component({
  selector: 'app-add-fine',
  standalone: true,
  imports: [
    FormsModule, ReactiveFormsModule, NgClass, NgForOf, NgIf
  ],
  templateUrl: './add-fine.component.html',
  styleUrl: './add-fine.component.css'
})
export class AddFineComponent implements OnInit {
  IsLoading: boolean = false;
  notification: boolean = false;
  IsSuccess: boolean = false;
  IsError: boolean = false;
  notificationMessage: string = '';
  form: FormGroup;
  denominations: { value: FineDenomination; key: number }[] = [];
  @Output() close = new EventEmitter<void>();
  @Input() CommunityID: number = 0;

  constructor(private fb: FormBuilder, private service: FineService) {
    this.form = this.fb.group({
      FineName: new FormControl('', Validators.required),
      FineAmount: new FormControl('', [Validators.required, Validators.min(0)]),
      Status: new FormControl('', [Validators.required]),
      CommunityID: new FormControl(0),
    });
  }


  ngOnInit(): void {
    this.populateDenominations();
  }

  populateDenominations(): void {
    this.denominations = Object.keys(FineDenomination)
      .filter(key => !isNaN(Number(key)))
      .map(key => ({
        key: Number(key),
        value: FineDenomination[key as keyof typeof FineDenomination]
      }));
  }


  onInputChange(controlName: string): void {
    const control = this.form.get(controlName);
    if (control) {
      control.markAsUntouched();
    }
  }

  onClose() {
    this.close.emit();
  }

  OnSubmit() {
    this.IsLoading = true;
    if(this.form.valid && this.CommunityID != 0) {
      const createFine : CreateFine= {
        name: this.form.value.FineName,
        amount: this.form.value.FineAmount,
        status: this.form.value.Status,
        communityId: this.CommunityID
      };
      this.service.createFine(createFine).subscribe({
        next: (response) => {
          if (response.status === 204) {
            this.IsLoading = false;
            this.notificationMessage = 'Multa agregada correctamente.';
            this.IsSuccess = true;
            this.notification = true;
            setTimeout(() => {
              this.notification = false;
              this.IsSuccess = false;
              this.IsLoading = false;
              this.close.emit();
            }, 3000);
          }
        },
        error: error => {
          this.IsLoading = false;
          if (error.status === 400) {
            const errorData = error.error?.[0];
            this.notificationMessage = errorData.message;
            this.IsError = true;
            this.notification = true;
            setTimeout(() => {
              this.notification = false;
              this.notificationMessage = '';
              this.IsError = false;
            }, 5000);
          } else {
            this.notificationMessage = 'No fue posible agregar la multa.';
            this.IsError = true;
            this.notification = true;
            setTimeout(() => {
              this.notification = false;
              this.notificationMessage = '';
              this.IsError = false;
            }, 5000);
          }
        },
      })



    }
  }

}
