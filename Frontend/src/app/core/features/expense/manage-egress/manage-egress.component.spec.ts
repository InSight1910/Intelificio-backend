import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageEgressComponent } from './manage-egress.component';

describe('ManageEgressComponent', () => {
  let component: ManageEgressComponent;
  let fixture: ComponentFixture<ManageEgressComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageEgressComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageEgressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
