import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Expense, ExpenseType } from '../../../../shared/models/expense.model';
import { FormatRutPipe } from '../../../../shared/pipes/format-rut/format-rut.pipe';
import { FormatRutDirective } from '../../../../shared/directives/format-rut/format-rut.directive';
import { ExpenseService } from '../../../services/expense/expense.service';
import { MessageComponent } from '../../../../shared/component/error/message.component';

@Component({
  selector: 'app-manage-egress',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormatRutDirective,
    MessageComponent,
  ],
  templateUrl: './manage-egress.component.html',
  styleUrl: './manage-egress.component.css',
})
export class ManageEgressComponent {
  fb: FormBuilder = inject(FormBuilder);
  expense: ExpenseService = inject(ExpenseService);

  expenseTypes: string[] = Object.values(ExpenseType).map((x: string) => x);
  isLoading: boolean = false;
  errors: { message: string }[] = [];

  form: FormGroup = this.fb.group({
    name: ['', Validators.required],
    amount: ['', [Validators.required, Validators.min(0)]],
    date: [new Date().toISOString().substring(0, 10), Validators.required],
    expenseType: ['', Validators.required],
    provider: ['', Validators.required],
    invoice: ['', Validators.required],
    pucharseOrder: ['', Validators.required],
  });

  onSubmit(event: Event): void {
    event.preventDefault();
    if (this.form.valid) {
      const expense: Expense = {
        ...this.form.value,
        providerRut: this.form
          .get('provider')!
          .value.replace(/[.\-]/g, '')
          .toUpperCase(),
      };
      this.isLoading = true;

      this.expense.create(this.form.value).subscribe({
        next: () => {
          this.isLoading = false;
          this.form.reset();
        },
        error: () => {
          this.isLoading = false;
        },
      });
    }
  }
}
