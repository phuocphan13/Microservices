import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryDeleteModalComponent } from './category-delete-modal.component';

describe('CategoryDeleteModalComponent', () => {
  let component: CategoryDeleteModalComponent;
  let fixture: ComponentFixture<CategoryDeleteModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryDeleteModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CategoryDeleteModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
