import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasicGridComponent } from './basic-grid.component';

describe('BasicGridComponent', () => {
  let component: BasicGridComponent;
  let fixture: ComponentFixture<BasicGridComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BasicGridComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BasicGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
