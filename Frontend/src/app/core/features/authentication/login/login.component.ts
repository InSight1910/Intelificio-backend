import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  loginForm: FormGroup;
  // loading$: Observable<boolean>;
  // error$: Observable<string | null>;

  constructor(private fb: FormBuilder) {
    this.loginForm = fb.group({
      username: [''],
      password: [''],
    });
  }

  onSubmit() {}
}
