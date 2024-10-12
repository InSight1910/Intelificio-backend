import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import {Router, RouterLink} from "@angular/router";

interface MenuSection {
  label: string;
  isOpen: boolean;
  items: MenuItem[];
}

interface MenuItem {
  label: string;
  iconClass: string;
  routerLink: string;
}

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css',
})
export class MenuComponent {
  @Input() openNavbar: boolean = false;
  isDropdownOpen: boolean = true;
  constructor(private router:Router) {}


  menuSections: MenuSection[] = [
    {
      label: 'Administración',
      isOpen: true,
      items: [
        { label: 'Comunidades', iconClass: 'fa-solid fa-tree-city', routerLink: 'AdminComunidades' },
        { label: 'Administrar Espacios Comunes', iconClass: 'fa-solid fa-people-roof', routerLink: 'AdminEspaciosComunes' },
        { label: 'Mantenciones', iconClass: 'fa-solid fa-toolbox', routerLink: 'Mantenciones' },
        { label: 'Edificios', iconClass: 'fa-solid fa-building', routerLink: 'Edificios' },
        { label: 'Comunicado', iconClass: 'fa-regular fa-paper-plane', routerLink: 'Comunicado' },
        { label: 'Registros de usuarios', iconClass: 'fa-solid fa-user-plus', routerLink: 'RegistroUsuario' },
        { label: 'Contactos', iconClass: 'fa-solid fa-address-book', routerLink: 'Contactos' },
      ],
    },
    {
      label: 'Residentes',
      isOpen: true,
      items: [
        { label: 'Comunidad', iconClass: 'fa-regular fa-building', routerLink: 'Comunidad' },
        { label: 'Espacios Comunes', iconClass: 'fa-solid fa-people-roof', routerLink: 'EspacioComun' },
        { label: 'Mis Reservas', iconClass: 'fa-solid fa-bookmark', routerLink: 'MisReservas' },
        { label: 'Edificios', iconClass: 'fa-solid fa-building', routerLink: 'Edificios' },
        { label: 'Contactos', iconClass: 'fa-solid fa-address-book', routerLink: 'Contactos' },
      ],
    },
    {
      label: 'Otros',
      isOpen: false,
      items: [
        { label: 'Reportes', iconClass: 'fa-solid fa-chart-bar', routerLink: 'Reportes' },
        { label: 'Configuración', iconClass: 'fa-solid fa-cogs', routerLink: 'Configuracion' },
      ],
    },
  ];


  toggleDropdown(section: MenuSection) {
    section.isOpen = !section.isOpen;
  }
}
