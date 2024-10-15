import { Component, inject, signal } from '@angular/core';
import { combineLatest, map, Observable } from 'rxjs';
import {
  MyPackages,
  Package,
  PackageStatus,
} from '../../../../shared/models/package.model';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../states/intelificio.state';
import { PackageService } from '../../../services/package/package.service';
import { selectCommunity } from '../../../../states/community/community.selectors';
import { selectUser } from '../../../../states/auth/auth.selectors';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserService } from '../../../services/user/user.service';
import { UserRut } from '../../../../shared/models/user.model';
import { ModalPackageComponent } from '../modal/modal.component';
import { FormatRutPipe } from '../../../../shared/pipes/format-rut/format-rut.pipe';
import { FormatRutDirective } from '../../../../shared/directives/format-rut/format-rut.directive';

@Component({
  selector: 'app-my-packages',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ModalPackageComponent,
    FormatRutPipe,
    FormatRutDirective,
  ],
  templateUrl: './my-packages.component.html',
  styleUrl: './my-packages.component.css',
})
export class MyPackagesComponent {
  store: Store<AppState> = inject(Store<AppState>);
  fb: FormBuilder = inject(FormBuilder);
  packageService: PackageService = inject(PackageService);
  userService: UserService = inject(UserService);

  isOpenModal = signal(new Map<number, boolean>());
  packages: Observable<MyPackages[]> = new Observable<MyPackages[]>();
  packageStatus = PackageStatus;
  form: FormGroup = this.fb.group({
    userId: ['', Validators.required],
    userRut: [''],
    userName: [{ value: '', disabled: true }],
    communityId: ['', Validators.required],
    packageId: ['', Validators.required],
  });

  errorMessageSearch: string = '';
  canMarkDelivered: boolean = false;

  ngOnInit() {
    combineLatest([
      this.store.select(selectCommunity),
      this.store.select(selectUser),
    ]).subscribe(([community, user]) => {
      if (community && user) {
        this.form.patchValue({ communityId: community.id });
        this.packages = this.packageService
          .getMyPackages(community.id!, user.sub!)
          .pipe(map(({ data }) => data));
      }
    });
  }

  onSearch(rut: string) {
    if (rut != '' && rut != '-') {
      this.store.select(selectCommunity).subscribe((community) => {
        if (!community) return;
        this.userService
          .getByRutCommunity(
            rut.replace(/[.\-]/g, '').toUpperCase(),
            community.id!
          )
          .subscribe({
            next: ({ data }) => {
              this.form.patchValue({
                userName: data.name,
                userId: data.id,
              });

              this.errorMessageSearch = '';
              this.canMarkDelivered = true;
            },
            error: ({ error }) => {
              this.errorMessageSearch = error[0].message;
              this.canMarkDelivered = false;
              this.form.patchValue({ userId: 0, userName: '' });
              this.form.get('conciergeId')?.disable();
              this.form.get('trackingNumber')?.disable();
            },
          });
      });
    }
  }

  onOpenAssignToRetire(packageId: number) {
    const value = this.isOpenModal();
    value.set(packageId, true);
    this.form.patchValue({ packageId });
  }

  onCloseAssignToRetire(packageId: number) {
    const value = this.isOpenModal();
    value.set(packageId, false);
    this.form.reset({ userId: 0, userName: '' });
  }

  onSubmit(event?: Event) {
    if (event) event.preventDefault();
    console.log(this.form.value);
    if (this.form.valid) {
      this.packageService
        .assingUserToRetire(
          this.form.value.communityId,
          this.form.value.packageId,
          this.form.value.userId
        )
        .subscribe({
          next: () => {
            this.onCloseAssignToRetire(this.form.value.packageId);
          },
          error: ({ error }) => {
            console.log(error);
          },
        });
    }
  }
}
