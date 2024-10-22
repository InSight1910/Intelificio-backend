import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FineComponent } from './fine.component';

describe('FineComponent', () => {
  let component: FineComponent;
  let fixture: ComponentFixture<FineComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FineComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
