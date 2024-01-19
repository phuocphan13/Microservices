import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryUpdateModalComponent } from './category-update-modal.component';

describe('CategoryUpdateModalComponent', () => {
  let component: CategoryUpdateModalComponent;
  let fixture: ComponentFixture<CategoryUpdateModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryUpdateModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CategoryUpdateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
