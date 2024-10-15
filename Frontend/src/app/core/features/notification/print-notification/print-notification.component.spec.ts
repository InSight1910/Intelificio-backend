import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrintNotificationComponent } from './print-notification.component';

describe('PrintNotificationComponent', () => {
  let component: PrintNotificationComponent;
  let fixture: ComponentFixture<PrintNotificationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PrintNotificationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PrintNotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
