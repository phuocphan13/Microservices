import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupcategorySearchComponent } from './supcategory-search.component';

describe('SupcategorySearchComponent', () => {
  let component: SupcategorySearchComponent;
  let fixture: ComponentFixture<SupcategorySearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SupcategorySearchComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SupcategorySearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
