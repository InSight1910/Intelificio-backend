import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormatRutDirective } from '../../../../shared/directives/format-rut/format-rut.directive';
import { FormatRutPipe } from '../../../../shared/pipes/format-rut/format-rut.pipe';
import { UserService } from '../../../services/user/user.service';

@Component({
  selector: 'app-manage',
  standalone: true,
  imports: [CommonModule, FormatRutDirective, FormatRutPipe],
  templateUrl: './manage.component.html',
  styleUrl: './manage.component.css',
})
export class ManageEncomiendasComponent {
  userService: UserService = inject(UserService);

  onSearch(rut: string) {
    this.userService
      .getByRut(rut.replace(/[.\-]/g, '').toUpperCase())
      .subscribe((response) => {
        console.log(response);
      });
  }
}
