import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangePasswordOneComponent } from './change-password-one.component';

describe('ChangePasswordOneComponent', () => {
  let component: ChangePasswordOneComponent;
  let fixture: ComponentFixture<ChangePasswordOneComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChangePasswordOneComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChangePasswordOneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
