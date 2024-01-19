import { Component, Input, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-category-view-modal',
  templateUrl: './category-view-modal.component.html',
  styleUrls: ['./category-view-modal.component.css']
})
export class CategoryViewModalComponent {
  @Input() modalData: any;

  constructor(public activeModal: NgbActiveModal) {
  }

  onClickClose() {
    this.activeModal.close();
  }
}
