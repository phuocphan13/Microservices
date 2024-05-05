import {Component, EventEmitter, Input, Output} from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-confirmation-modal',
  templateUrl: './confirmation-modal.component.html',
  styleUrl: './confirmation-modal.component.css'
})
export class ConfirmationModalComponent {
  @Input() name: string = "";
  @Input() title: string = "";
  @Output() emiter = new EventEmitter<boolean>();

  constructor(private activeModal: NgbActiveModal) {
  }

  onClickClose() {
    this.emiter.emit(false);
    this.activeModal.close();
  }

  onClickConfirm() {
    this.emiter.emit(true);
    this.activeModal.close();
  }
}
