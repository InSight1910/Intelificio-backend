import {Component, OnInit} from '@angular/core';
import {Contact} from "../../../shared/models/contact.model";
import {FormsModule} from "@angular/forms";
import {AddContactComponent} from "./add-contact/add-contact.component";
import {Store} from "@ngrx/store";
import {AppState} from "../../../states/intelificio.state";
import {ContactService} from "../../services/contact/contact.service";
import {selectCommunity} from "../../../states/community/community.selectors";
import {tap} from "rxjs";

@Component({
  selector: 'app-contact-list',
  standalone: true,
  imports: [
    FormsModule,
    AddContactComponent,
  ],
  templateUrl: './contact-list.component.html',
  styleUrl: './contact-list.component.css'
})
export class ContactListComponent implements OnInit {
  isModalOpen: boolean = false;
  isLoading: boolean = false;
  contactos: Contact[] = []
  filteredContacts: Contact[] = [];
  CommunityID: number = 0;
  searchTerm: string = '';

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

  openAddModal() {
    this.isModalOpen = true;
  }

  filterContacts() {
    if (!this.searchTerm) {
      this.filteredContacts = this.contactos;
      return;
    }

    const searchTermLower = this.searchTerm.toLowerCase();
    this.filteredContacts = this.contactos.filter(contact =>
      contact.name.toLowerCase().includes(searchTermLower) ||
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
                console.log('Contactos:', this.contactos); // Aquí revisas si los datos son correctos
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


}
