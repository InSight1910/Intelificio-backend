import { Component } from '@angular/core';
import '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { AuthActions } from '../../../../states/auth/auth.actions';
import {
  selectError,
  selectLoading,
} from '../../../../states/auth/auth.selectors';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  loginForm: FormGroup;
  loading$!: Observable<boolean>;
  errors$!: Observable<string[] | null>;

  constructor(private fb: FormBuilder, private store: Store) {
    this.loginForm = this.fb.group({
      username: [''],
      password: [''],
    });
  }

  onSubmit() {
    this.loading$ = this.store.select(selectLoading);

    this.store.dispatch(
      AuthActions.login({
        email: this.loginForm.get('username')?.value,
        password: this.loginForm.get('password')?.value,
      })
    );

    this.errors$ = this.store.select(selectError);
  }
}
