import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyPackagesComponent } from './my-packages.component';

describe('MyPackagesComponent', () => {
  let component: MyPackagesComponent;
  let fixture: ComponentFixture<MyPackagesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyPackagesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyPackagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
