import { Component } from '@angular/core';

@Component({
  selector: 'app-manage',
  standalone: true,
  imports: [],
  templateUrl: './manage.component.html',
  styleUrl: './manage.component.css',
})
export class ManageComponent {
  items: number[] = [1, 2, 3, 4, 5];
}
