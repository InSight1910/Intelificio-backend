import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-print-notification',
  standalone: true,
  imports: [],
  templateUrl: './print-notification.component.html',
  styleUrl: './print-notification.component.css'
})
export class PrintNotificationComponent  {
  @Input() notificationData: any;

  constructor() {}



}
