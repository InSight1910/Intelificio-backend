import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditFineComponent } from './edit-fine.component';

describe('EditFineComponent', () => {
  let component: EditFineComponent;
  let fixture: ComponentFixture<EditFineComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditFineComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditFineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
