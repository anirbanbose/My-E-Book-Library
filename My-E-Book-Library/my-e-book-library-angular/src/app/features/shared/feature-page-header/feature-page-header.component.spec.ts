import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeaturePageHeaderComponent } from './feature-page-header.component';

describe('FeaturePageHeaderComponent', () => {
  let component: FeaturePageHeaderComponent;
  let fixture: ComponentFixture<FeaturePageHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FeaturePageHeaderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FeaturePageHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
