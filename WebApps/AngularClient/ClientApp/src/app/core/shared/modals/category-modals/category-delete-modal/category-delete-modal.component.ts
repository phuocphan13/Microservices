import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-category-delete-modal',
  templateUrl: './category-delete-modal.component.html',
  styleUrls: ['./category-delete-modal.component.css']
})
export class CategoryDeleteModalComponent {

  @Input() modalData: any;

  constructor(public activeModal: NgbActiveModal) {
  }

  onClickDelete(){
    this.activeModal.close();
  }
  onClickClose() {
    this.activeModal.close();
  }
}
