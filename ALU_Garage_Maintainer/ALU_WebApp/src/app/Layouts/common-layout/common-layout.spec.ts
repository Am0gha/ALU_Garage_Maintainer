import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommonLayout } from './common-layout';

describe('CommonLayout', () => {
  let component: CommonLayout;
  let fixture: ComponentFixture<CommonLayout>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CommonLayout]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CommonLayout);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
