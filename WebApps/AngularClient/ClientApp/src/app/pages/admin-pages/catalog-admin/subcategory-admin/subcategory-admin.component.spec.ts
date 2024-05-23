import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubcategoryAdminComponent } from './subcategory-admin.component';

describe('SubcategoryAdminComponent', () => {
  let component: SubcategoryAdminComponent;
  let fixture: ComponentFixture<SubcategoryAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubcategoryAdminComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SubcategoryAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
