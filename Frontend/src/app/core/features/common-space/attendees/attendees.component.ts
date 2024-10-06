import { Component } from '@angular/core';
import { ModalComponent } from "../modal/modal.component";

@Component({
  selector: 'app-attendees',
  standalone: true,
  imports: [ModalComponent],
  templateUrl: './attendees.component.html',
  styleUrl: './attendees.component.css'
})
export class AttendeesComponent {

}
