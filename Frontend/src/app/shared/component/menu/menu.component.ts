import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import {Router, RouterLink} from "@angular/router";

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css',
})
export class MenuComponent {
  @Input() openNavbar: boolean = false;
  constructor(private router:Router) {}

}
