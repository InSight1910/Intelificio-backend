import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsersCommunityComponent } from './users-community.component';

describe('UsersCommunityComponent', () => {
  let component: UsersCommunityComponent;
  let fixture: ComponentFixture<UsersCommunityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UsersCommunityComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UsersCommunityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
