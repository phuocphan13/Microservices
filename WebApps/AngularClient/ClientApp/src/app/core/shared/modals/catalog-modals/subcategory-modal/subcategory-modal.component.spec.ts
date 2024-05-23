import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubcategoryModalComponent } from './subcategory-modal.component';

describe('SubcategoryModalComponent', () => {
  let component: SubcategoryModalComponent;
  let fixture: ComponentFixture<SubcategoryModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubcategoryModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SubcategoryModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
