import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-category-update-modal',
  templateUrl: './category-update-modal.component.html',
  styleUrls: ['./category-update-modal.component.css']
})
export class CategoryUpdateModalComponent {
  
  @Input() modalData: any;
  
  constructor(public activeModal: NgbActiveModal) {
  }

  onSubmit(){
    //call API
    this.activeModal.close();
  }
  onClickClose() {
    this.activeModal.close();
  }
}
