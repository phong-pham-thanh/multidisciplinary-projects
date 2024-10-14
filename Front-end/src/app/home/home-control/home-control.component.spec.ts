import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeControlComponent } from './home-control.component';

describe('HomeControlComponent', () => {
  let component: HomeControlComponent;
  let fixture: ComponentFixture<HomeControlComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HomeControlComponent]
    });
    fixture = TestBed.createComponent(HomeControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
