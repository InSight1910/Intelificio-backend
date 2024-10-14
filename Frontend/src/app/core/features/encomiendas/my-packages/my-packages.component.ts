import { Component, inject } from '@angular/core';
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

@Component({
  selector: 'app-my-packages',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-packages.component.html',
  styleUrl: './my-packages.component.css',
})
export class MyPackagesComponent {
  store: Store<AppState> = inject(Store<AppState>);
  packageService: PackageService = inject(PackageService);

  packages: Observable<MyPackages[]> = new Observable<MyPackages[]>();
  packageStatus = PackageStatus;

  ngOnInit() {
    combineLatest([
      this.store.select(selectCommunity),
      this.store.select(selectUser),
    ]).subscribe(([community, user]) => {
      if (community && user) {
        this.packages = this.packageService
          .getMyPackages(community.id!, user.sub!)
          .pipe(map(({ data }) => data));
      }
    });
  }
}
