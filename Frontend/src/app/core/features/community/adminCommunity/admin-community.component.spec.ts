import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCommunityComponent } from './admin-community.component';

describe('AdminCommunityComponent', () => {
  let component: AdminCommunityComponent;
  let fixture: ComponentFixture<AdminCommunityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminCommunityComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminCommunityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
