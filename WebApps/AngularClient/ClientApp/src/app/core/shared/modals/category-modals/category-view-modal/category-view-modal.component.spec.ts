import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryViewModalComponent } from './category-view-modal.component';

describe('CategoryViewModalComponent', () => {
  let component: CategoryViewModalComponent;
  let fixture: ComponentFixture<CategoryViewModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryViewModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CategoryViewModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
