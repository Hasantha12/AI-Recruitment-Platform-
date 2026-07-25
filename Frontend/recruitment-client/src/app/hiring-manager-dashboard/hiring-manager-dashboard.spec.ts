import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HiringManagerDashboard } from './hiring-manager-dashboard';

describe('HiringManagerDashboard', () => {
  let component: HiringManagerDashboard;
  let fixture: ComponentFixture<HiringManagerDashboard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HiringManagerDashboard],
    }).compileComponents();

    fixture = TestBed.createComponent(HiringManagerDashboard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
