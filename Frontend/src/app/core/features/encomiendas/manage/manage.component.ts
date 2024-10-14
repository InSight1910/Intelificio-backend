import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  inject,
  signal,
  ViewChild,
} from '@angular/core';
import { FormatRutDirective } from '../../../../shared/directives/format-rut/format-rut.directive';
import { FormatRutPipe } from '../../../../shared/pipes/format-rut/format-rut.pipe';
import { UserService } from '../../../services/user/user.service';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../states/intelificio.state';
import { selectCommunity } from '../../../../states/community/community.selectors';
import { UserRut } from '../../../../shared/models/user.model';
import { map, Observable, of } from 'rxjs';
import {
  CreatePackage,
  Package,
  PackageStatus,
} from '../../../../shared/models/package.model';
import { PackageService } from '../../../services/package/package.service';
import { ModalPackageComponent } from '../modal/modal.component';

@Component({
  selector: 'app-package-manage',
  standalone: true,
  imports: [
    CommonModule,
    FormatRutDirective,
    FormatRutPipe,
    ReactiveFormsModule,
    ModalPackageComponent,
  ],
  templateUrl: './manage.component.html',
  styleUrl: './manage.component.css',
})
export class ManageEncomiendasComponent {
  userService: UserService = inject(UserService);
  packageService: PackageService = inject(PackageService);
  fb: FormBuilder = inject(FormBuilder);
  store: Store<AppState> = inject(Store<AppState>);
  recipient!: UserRut;
  errorMessageSearch: string = '';
  openMarkAsDelivered = signal(new Map<number, boolean>());
  selectedPackageId: number = 0;
  deliveredToId: number = 0;
  @ViewChild('recipientName') recipientName!: ElementRef;

  form: FormGroup = this.fb.group(
    {
      recipientId: ['', Validators.required],
      recipientRut: [''],
      conciergeId: ['', Validators.required],
      trackingNumber: ['', Validators.required],
    },
    [Validators.required]
  );

  formMarkDelivered: FormGroup = this.fb.group({
    deliveryToRut: ['', Validators.required],
    deliveryToName: [{ value: '', disabled: true }, Validators.required],
    deliveryToId: [''],
  });

  concierges: Observable<UserRut[]> = new Observable<UserRut[]>();
  packages: Observable<Package[]> = new Observable<Package[]>();
  packageStatus = PackageStatus;
  canCreate: boolean = false;
  canMarkDelivered: boolean = false;

  ngOnInit() {
    this.loadConcierges();
    this.loadPackages();
    this.form.get('conciergeId')?.disable();
    this.form.get('trackingNumber')?.disable();
  }

  onSearch(rut: string, isMarkDelivered: boolean = false) {
    this.store.select(selectCommunity).subscribe((community) => {
      this.userService
        .getByRutCommunity(
          rut.replace(/[.\-]/g, '').toUpperCase(),
          community?.id!
        )
        .subscribe({
          next: ({ data }) => {
            if (isMarkDelivered) {
              this.formMarkDelivered.patchValue({
                deliveryToName: data.name,
                deliveryToId: data.id,
              });
              this.canMarkDelivered = true;
              return;
            }
            this.recipient = data;
            this.form.patchValue({ recipientId: data.id });
            this.errorMessageSearch = '';
            this.canCreate = true;
            this.form.get('conciergeId')?.enable();
            this.form.get('trackingNumber')?.enable();
          },
          error: ({ error }) => {
            console.log(error);
            this.errorMessageSearch = error[0].message;
            this.canCreate = false;
            this.canMarkDelivered = false;
            this.form.get('conciergeId')?.disable();
            this.form.get('trackingNumber')?.disable();
          },
        });
    });
  }

  onSubmit(event: Event) {
    event.preventDefault();
    if (this.form.invalid) return;
    this.store.select(selectCommunity).subscribe((community) => {
      const packages: CreatePackage = {
        ...this.form.value,
        communityId: community?.id!,
        recipientId: this.recipient.id,
      };
      this.packageService.create(packages).subscribe(({ data }) => {
        this.packages = this.packages.pipe(map((packages) => packages));
        this.form.reset();
        this.recipient = {} as UserRut;
      });
    });

    this.canCreate = false;
  }

  loadConcierges() {
    this.store.select(selectCommunity).subscribe((community) => {
      this.concierges = this.userService
        .getConcierges(community?.id!)
        .pipe(map(({ data }) => data));
    });
  }

  loadPackages() {
    this.store.select(selectCommunity).subscribe((community) => {
      this.packages = this.packageService
        .getPackages(community?.id!)
        .pipe(map(({ data }) => data));
    });
  }

  openModalMarkAsDelivered(id: number) {
    var packageSelected = this.openMarkAsDelivered();
    packageSelected.set(id, true);

    this.selectedPackageId = id;
  }

  closeModalMarkAsDelivered(id: number) {
    var packageSelected = this.openMarkAsDelivered();
    packageSelected.delete(id);

    this.selectedPackageId = 0;
  }

  onSubmitMarkDelivered(event?: Event | null) {
    console.log(this.formMarkDelivered.invalid, this.selectedPackageId);
    console.log(this.formMarkDelivered.value);
    if (this.formMarkDelivered.invalid || this.selectedPackageId == 0) return;
    this.packageService
      .markAsDelivered(
        this.selectedPackageId,
        this.formMarkDelivered.get('deliveryToId')?.value
      )
      .subscribe({
        next: (response) => {
          this.loadPackages();
          this.closeModalMarkAsDelivered(this.selectedPackageId);
          this.formMarkDelivered.reset();
        },
      });
  }
}
