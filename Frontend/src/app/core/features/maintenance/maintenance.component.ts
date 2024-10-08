import {Component, OnInit} from '@angular/core';
import {FormsModule} from "@angular/forms";
import {DatePipe, NgClass, NgForOf, NgIf} from "@angular/common";
import {Maintenance} from "../../../shared/models/maintenance.model";
import {Store} from "@ngrx/store";
import {AppState} from "../../../states/intelificio.state";
import {MaintenanceService} from "../../services/maintenance/maintenance.service";
import {selectCommunity} from "../../../states/community/community.selectors";
import {tap} from "rxjs";
import {Building} from "../../../shared/models/building.model";
import {Community} from "../../../shared/models/community.model";
import {AddContactComponent} from "../contact-list/add-contact/add-contact.component";
import {EditContactComponent} from "../contact-list/edit-contact/edit-contact.component";

@Component({
  selector: 'app-maintenance',
  standalone: true,
  imports: [
    FormsModule,
    NgClass,
    DatePipe,
    NgIf,
    NgForOf,
    AddContactComponent,
    EditContactComponent
  ],
  templateUrl: './maintenance.component.html',
  styleUrl: './maintenance.component.css'
})
export class MaintenanceComponent implements OnInit {
  searchTerm: string = '';
  startDate: Date | null = null;
  endDate: Date | null = null;
  showStartDatePicker: boolean = false;
  showEndDatePicker: boolean = false;

  maintenances!: Maintenance[];
  filteredMaintenances!: Maintenance[];

  community!: Community | null;
  isLoading = false;

  constructor(private service: MaintenanceService,private store: Store<AppState>) {}

  ngOnInit(){
    this.getallMaintenance();
  }

  getallMaintenance(){
    this.isLoading = true;
    this.store
      .select(selectCommunity)
      .pipe(
        tap((c) => {
          this.community = c;
          this.service.getbyCommunityId(c?.id!).subscribe(
            (response: { data: Maintenance[] }) => {
              this.maintenances = response.data;
              this.filteredMaintenances = this.maintenances;
              this.isLoading = false;
            },
            (error) => {
              this.isLoading = false;
              console.error('Error al obtener mantenciones.', error);
            }
          );
        })
      )
      .subscribe();
  }


  onSearchChange() {
    if (!this.searchTerm) {
      this.filteredMaintenances = this.maintenances;
      return;
    }

    const searchTermLower = this.searchTerm.toLowerCase();
    this.filteredMaintenances = this.maintenances.filter(Maintenance =>
      Maintenance.commonSpaceName.toLowerCase().includes(searchTermLower) ||
      Maintenance.commonSpaceLocation.toLowerCase().includes(searchTermLower) ||
      Maintenance.comment.toLowerCase().includes(searchTermLower) ||
      Maintenance.startDate.includes(this.searchTerm) ||
      Maintenance.endDate.includes(searchTermLower)
    );
  }

}
