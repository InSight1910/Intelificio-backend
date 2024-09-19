import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup;
  // loading$: Observable<boolean>;
  // error$: Observable<string | null>;

  constructor(private fb: FormBuilder) {
    this.loginForm = fb.group({
      username: [''],
      password: ['']
    });
  }

  onSubmit() {}

}
