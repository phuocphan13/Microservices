import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryCreateModalComponent } from './category-create-modal.component';

describe('CategoryCreateModalComponent', () => {
  let component: CategoryCreateModalComponent;
  let fixture: ComponentFixture<CategoryCreateModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryCreateModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CategoryCreateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
