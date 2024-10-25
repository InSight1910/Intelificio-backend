import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyFinesComponent } from './my-fines.component';

describe('MyFinesComponent', () => {
  let component: MyFinesComponent;
  let fixture: ComponentFixture<MyFinesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyFinesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyFinesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
