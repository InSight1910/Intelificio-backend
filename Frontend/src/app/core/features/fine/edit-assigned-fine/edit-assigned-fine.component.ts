import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {AssignedFine, Fine, FineDenomination} from "../../../../shared/models/fine.model";
import {FineService} from "../../../services/fine/fine.service";

@Component({
  selector: 'app-edit-assigned-fine',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './edit-assigned-fine.component.html',
  styleUrl: './edit-assigned-fine.component.css'
})
export class EditAssignedFineComponent implements OnInit {
  IsLoading: boolean = false;
  notification: boolean = false;
  IsSuccess: boolean = false;
  IsError: boolean = false;
  notificationMessage: string = '';
  form: FormGroup;
  denominations: { value: FineDenomination; key: number }[] = [];
  @Output() close = new EventEmitter<void>();
  @Input() assignedFine!: AssignedFine;

  constructor(private fb: FormBuilder, private service: FineService) {
    this.form = this.fb.group({
      fineName: new FormControl('', Validators.required),
      eventDate: new FormControl('', [Validators.required]),
      amount: new FormControl('', [Validators.required, Validators.min(0)]),
      status: new FormControl('', [Validators.required]),
      assignedFineId: new FormControl(0),
      fineId: new FormControl(0),
      unitId: new FormControl(0),
      communityId: new FormControl(0),
    });
  }

  onClose() {
    this.close.emit();
  }

  ngOnInit(): void {
    this.form.patchValue(this.assignedFine);
  }

  onInputChange(controlName: string): void {
    const control = this.form.get(controlName);
    if (control) {
      control.markAsUntouched();
    }
  }

  OnSubmit(){

  }
  delete(){

  }

}
