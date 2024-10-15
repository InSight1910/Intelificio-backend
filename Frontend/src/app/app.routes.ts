import { Routes } from '@angular/router';
import { LoginComponent } from './core/features/authentication/login/login.component';
import { SelectCommunityComponent } from './core/features/community/select-community/select-community.component';
import { HomeCommunityComponent } from './core/features/community/home/home.component';
import { UnitComponent } from './core/features/unit/unit.component';
import { BuildingComponent } from './core/features/building/building.component';
import { SingupComponent } from './core/features/authentication/signup/signup.component';
import { ChangePasswordOneComponent } from './core/features/authentication/change-password-one/change-password-one.component';
import { ChangePasswordTwoComponent } from './core/features/authentication/change-password-two/change-password-two.component';
import { AdminCommunityComponent } from './core/features/community/adminCommunity/admin-community.component';
import { HomeSpaceComponent } from './core/features/common-space/home/home.component';
import { ManageComponent } from './core/features/common-space/manage/manage.component';
import { NotificationComponent } from './core/features/notification/notification.component';
import { MaintenanceComponent } from './core/features/maintenance/maintenance.component';
import { ContactListComponent } from './core/features/contact-list/contact-list.component';
import { ConfirmReservationComponent } from './core/features/reservation/confirm-reservation/confirm-reservation.component';
import { MyReservationsComponent } from './core/features/reservation/my-reservations/my-reservations.component';
import { ConfirmEmailComponent } from './core/features/authentication/confirm-email/confirm-email.component';
import { ManageEncomiendasComponent } from './core/features/encomiendas/manage/manage.component';
import { MyPackagesComponent } from './core/features/encomiendas/my-packages/my-packages.component';
import { PrintNotificationComponent} from "./core/features/notification/print-notification/print-notification.component";
import { GuestComponent } from './core/features/guest/guest.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'Edificios',
    component: BuildingComponent,
  },
  {
    path: 'RegistroUsuario',
    component: SingupComponent,
  },
  {
    path: 'forgot-password',
    component: ChangePasswordOneComponent,
  },
  {
    path: 'change-password',
    component: ChangePasswordTwoComponent,
  },
  // {
  //   path: '**',
  //   redirectTo: 'login',
  // },
  {
    path: 'select-community',
    component: SelectCommunityComponent,
  },
  {
    path: 'Comunidad',
    component: HomeCommunityComponent,
  },
  {
    path: 'unit',
    component: UnitComponent,
  },
  {
    path: 'AdminComunidades',
    component: AdminCommunityComponent,
  },
  {
    path: 'AdminEspaciosComunes',
    component: ManageComponent,
  },
  {
    path: 'EspacioComun',
    component: HomeSpaceComponent,
  },
  {
    path: 'Comunicado',
    component: NotificationComponent,
  },
  {
    path: 'Mantenciones',
    component: MaintenanceComponent,
  },
  {
    path: 'ConfirmarReserva',
    component: ConfirmReservationComponent,
  },
  {
    path: 'MisReservas',
    component: MyReservationsComponent,
  },
  {
    path: 'ConfirmarCorreo',
    component: ConfirmEmailComponent,
  },
  {
    path: 'Contactos',
    component: ContactListComponent,
  },
  {
    path: 'RegistroEncomienda',
    component: ManageEncomiendasComponent,
  },
  {
    path: 'MisEncomiendas',
    component: MyPackagesComponent,
  },
  {
    path: 'print-notification',
    component: PrintNotificationComponent,
  },
  {
    path: 'Visitas',
    component: GuestComponent,
  },
];
