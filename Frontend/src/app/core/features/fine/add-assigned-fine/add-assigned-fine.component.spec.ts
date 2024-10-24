import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAssignedFineComponent } from './add-assigned-fine.component';

describe('AddAssignedFineComponent', () => {
  let component: AddAssignedFineComponent;
  let fixture: ComponentFixture<AddAssignedFineComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddAssignedFineComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddAssignedFineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
