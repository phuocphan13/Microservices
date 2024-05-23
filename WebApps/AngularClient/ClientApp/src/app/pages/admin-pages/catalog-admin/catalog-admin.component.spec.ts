import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogAdminComponent } from './catalog-admin.component';

describe('CatalogAdminComponent', () => {
  let component: CatalogAdminComponent;
  let fixture: ComponentFixture<CatalogAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CatalogAdminComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CatalogAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
