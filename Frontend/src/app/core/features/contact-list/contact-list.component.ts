import {Component, HostListener, OnInit} from '@angular/core';
import {Contact} from "../../../shared/models/contact.model";
import {FormsModule} from "@angular/forms";
import {AddContactComponent} from "./add-contact/add-contact.component";
import {Store} from "@ngrx/store";
import {AppState} from "../../../states/intelificio.state";
import {ContactService} from "../../services/contact/contact.service";
import {selectCommunity} from "../../../states/community/community.selectors";
import {tap} from "rxjs";
import {EditContactComponent} from "./edit-contact/edit-contact.component";
import {NgClass} from "@angular/common";

@Component({
  selector: 'app-contact-list',
  standalone: true,
  imports: [
    FormsModule,
    AddContactComponent,
    EditContactComponent,
    NgClass,
  ],
  templateUrl: './contact-list.component.html',
  styleUrl: './contact-list.component.css'
})
export class ContactListComponent implements OnInit {
  isModalOpen: boolean = false;
  isEditModalOpen: boolean = false;
  isDropdownOpen: { [key: number]: boolean } = {};
  isLoading: boolean = false;
  contactos: Contact[] = []
  filteredContacts: Contact[] = [];
  CommunityID: number = 0;
  searchTerm: string = '';
  contactToEdit:  Contact = {
    communityID: 0,
    email: "",
    firstName: "",
    id: 0,
    lastName: "",
    phoneNumber: "",
    service: ""
  };

  constructor(
    private service: ContactService,
    private store: Store<AppState>
  ) {}

  ngOnInit() {
    this.getContacts();
  }

  onClickCloseAdd() {
    this.isModalOpen = false;
  }

  onClickCloseEdit(){
    this.isEditModalOpen = false;
  }

  openAddModal() {
    this.isModalOpen = true;
  }

  openEditModal(contact: Contact) {
    this.contactToEdit = contact;
    this.isEditModalOpen = true;
  }

  filterContacts() {
    if (!this.searchTerm) {
      this.filteredContacts = this.contactos;
      return;
    }

    const searchTermLower = this.searchTerm.toLowerCase();
    this.filteredContacts = this.contactos.filter(contact =>
      contact.firstName.toLowerCase().includes(searchTermLower) ||
      contact.lastName.toLowerCase().includes(searchTermLower) ||
      contact.phoneNumber.includes(this.searchTerm) ||
      contact.service.toLowerCase().includes(searchTermLower)
    );
  }

  getContacts(){
    this.isLoading = true;
    this.store
      .select(selectCommunity)
      .pipe(
        tap((c) => {
          this.CommunityID = c?.id ?? 0;
          this.service.getbyCommunityId(c?.id!).subscribe(
            {
              next: (response: { data: Contact[] }) => {
                this.contactos = response.data;
                this.filteredContacts = this.contactos;
                this.isLoading = false;
              },
              error: (error) => {
                this.isLoading = false;
                console.error('Error al obtener contactos de comunidad', error);
              }
            }
          );
        })
      )
      .subscribe();
  }

  @HostListener('document:click', ['$event'])
  handleClickOutside(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (!target.closest('.dropdown')) {
      Object.keys(this.isDropdownOpen).forEach((id) => {
        this.isDropdownOpen[+id] = false;
      });
    }
  }


}
