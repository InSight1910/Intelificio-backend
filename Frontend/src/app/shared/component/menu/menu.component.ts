import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import {Router, RouterLink, RouterLinkActive} from "@angular/router";

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
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css',
})
export class MenuComponent {
  @Input() openNavbar: boolean = false;

  constructor(private router:Router) {}



  menuSections: MenuSection[] = [
    {
      label: 'Super-Admin',
      isOpen: true,
      items: [
        { label: 'Comunidades', iconClass: 'fa-solid fa-tree-city', routerLink: 'AdminComunidades' },
      ],
    },
    {
      label: 'Administración',
      isOpen: true,
      items: [
        { label: 'Comunidad', iconClass: 'fa-solid fa-tree-city', routerLink: 'Comunidad' },
        { label: 'Edificios', iconClass: 'fa-solid fa-building', routerLink: 'EdificiosComunidad' },
        { label: 'Administrar Espacios Comunes', iconClass: 'fa-solid fa-people-roof', routerLink: 'AdminEspaciosComunes' },
        { label: 'Mantenciones', iconClass: 'fa-solid fa-toolbox', routerLink: 'Mantenciones' },
        { label: 'Comunicado', iconClass: 'fa-solid fa-paper-plane', routerLink: 'Comunicado' },
        { label: 'Registro de usuarios', iconClass: 'fa-solid fa-user-plus', routerLink: 'RegistroUsuario' },
        { label: 'Registro de Encomiendas', iconClass: 'fa-solid fa-truck-fast', routerLink: 'RegistroEncomienda' },
        { label: 'Contactos de la Comunidad', iconClass: 'fa-solid fa-address-book', routerLink: 'ContactosComunidad' },
        { label: 'Visitas', iconClass: 'fa-solid fa-person-walking', routerLink: 'Visitas' },
        { label: 'Multas', iconClass: 'fa-solid fa-file-invoice', routerLink: 'Multas' },
      ],
    },
    {
      label: 'Residentes',
      isOpen: true,
      items: [
        { label: 'Mi Comunidad', iconClass: 'fa-solid fa-tree-city', routerLink: 'MiComunidad' },
        { label: 'Edificios', iconClass: 'fa-solid fa-city', routerLink: 'Edificios' },
        { label: 'Espacios Comunes', iconClass: 'fa-solid fa-people-roof', routerLink: 'EspacioComun' },
        { label: 'Mis Reservas', iconClass: 'fa-solid fa-bookmark', routerLink: 'MisReservas' },
        { label: 'Mis Encomiendas', iconClass: 'fa-solid fa-box', routerLink: 'MisEncomiendas' },
        { label: 'Mis Multas', iconClass: 'fa-solid fa-file-invoice', routerLink: 'MisMultas' },
        { label: 'Contactos', iconClass: 'fa-solid fa-address-book', routerLink: 'Contactos' },
      ],
    },
    {
      label: 'Otros',
      isOpen: true,
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
