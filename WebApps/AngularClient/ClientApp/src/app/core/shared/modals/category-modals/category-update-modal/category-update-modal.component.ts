import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-category-update-modal',
  templateUrl: './category-update-modal.component.html',
  styleUrls: ['./category-update-modal.component.css']
})
export class CategoryUpdateModalComponent {

  @Input() modalData: any;
  @Output() closeModal = new EventEmitter<void>();
  @Output() updateData = new EventEmitter<any>(); 

  constructor(public activeModal: NgbActiveModal) {
  }

  onCloseModal(): void {
    this.closeModal.emit();
  }

  onSubmit(): void {
    this.updateData.emit(this.modalData);

    this.onCloseModal();
  }
  onClickClose() {
    this.activeModal.close();
  }
}
