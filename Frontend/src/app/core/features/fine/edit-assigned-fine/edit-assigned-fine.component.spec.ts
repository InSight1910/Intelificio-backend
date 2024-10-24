import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditAssignedFineComponent } from './edit-assigned-fine.component';

describe('EditAssignedFineComponent', () => {
  let component: EditAssignedFineComponent;
  let fixture: ComponentFixture<EditAssignedFineComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditAssignedFineComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditAssignedFineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
