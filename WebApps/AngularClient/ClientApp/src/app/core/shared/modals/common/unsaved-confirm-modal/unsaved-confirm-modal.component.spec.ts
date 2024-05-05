import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UnsavedConfirmModalComponent } from './unsaved-confirm-modal.component';

describe('UnsavedConfirmModalComponent', () => {
  let component: UnsavedConfirmModalComponent;
  let fixture: ComponentFixture<UnsavedConfirmModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UnsavedConfirmModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UnsavedConfirmModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
