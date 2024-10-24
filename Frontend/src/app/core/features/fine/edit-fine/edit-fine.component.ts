import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {Fine, FineDenomination, UpdateFine} from "../../../../shared/models/fine.model";
import {FineService} from "../../../services/fine/fine.service";
import {NgClass, NgForOf, NgIf} from "@angular/common";

@Component({
  selector: 'app-edit-fine',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './edit-fine.component.html',
  styleUrl: './edit-fine.component.css'
})
export class EditFineComponent implements OnInit {
  IsLoading: boolean = false;
  notification: boolean = false;
  IsSuccess: boolean = false;
  IsError: boolean = false;
  notificationMessage: string = '';
  form: FormGroup;
  denominations: { value: FineDenomination; key: number }[] = [];
  @Output() close = new EventEmitter<void>();
  @Input() fine!: Fine;

  constructor(private fb: FormBuilder, private service: FineService) {
    this.form = this.fb.group({
      name: new FormControl('', Validators.required),
      amount: new FormControl('', [Validators.required, Validators.min(0)]),
      status: new FormControl('', [Validators.required]),
      communityId: new FormControl(0),
    });
  }

  ngOnInit(): void {
    this.populateDenominations();
    this.form.patchValue(this.fine);
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

  OnSubmit(){
    this.IsLoading = true;
    if(this.form.valid && this.fine.communityId != 0){
      const updateFine: UpdateFine = {
        fineId: this.fine.fineId,
        name: this.form.value.name,
        amount: this.form.value.amount,
        status: this.form.value.status,
        communityId: this.fine.communityId
      };
      this.service.updateFine(this.fine.fineId, updateFine).subscribe({
        next: (response) => {
          if (response.status === 200) {
            this.IsLoading = false;
            this.notificationMessage = 'Multa actualizada correctamente.';
            this.IsSuccess = true;
            this.notification = true;
            setTimeout(() => {
              this.notification = false;
              this.IsSuccess = false;
              this.IsLoading = false;
              this.close.emit();
            },3000);
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
              this.close.emit();
            }, 4000);
          } else {
            this.notificationMessage = 'No fue posible actualizar la multa.';
            this.IsError = true;
            this.notification = true;
            setTimeout(() => {
              this.notification = false;
              this.notificationMessage = '';
              this.IsError = false;
            }, 4000);
          }
        },
      });
    }
  }


}
