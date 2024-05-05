import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { Action } from "../../../../../common/const";

@Component({
  selector: 'app-category-modal',
  templateUrl: './category-modal.component.html',
  styleUrls: ['./category-modal.component.css']
})
export class CategoryModalComponent implements OnInit {
  @Input() modalData: any;
  @Input() type: string = Action.View;
  title: string = "";
  isDisabled: boolean = false;
  formData: any = {};
  @Output() successEvent = new EventEmitter<any>();
  @Output() closeEvent = new EventEmitter<void>();

  constructor(public activeModal: NgbActiveModal) { }

  ngOnInit(): void {
    this.setTitle();
    this.setDisabledState();
  }

  setTitle() {
    if (this.type == 'edit') {
      this.title = 'Edit Category';
    } else if (this.type == 'create') {
      this.title = 'Create New Category';
    } else if (this.type == 'view') {
      this.title = 'View Category';
    } else {
      this.title = '';
    }
  }

  async onSubmit() {
    if (this.isValidForm()) {
      if (this.type === Action.Edit || this.type === Action.Create) {
        this.successEvent.emit(this.formData);
        this.activeModal.close();
      }
    }
  }

  onClickClose() {
    this.activeModal.close();
    this.closeEvent.emit();
  }

  setDisabledState() {
    this.isDisabled = this.type === 'view';
  }

  isValidName(name: string): boolean {
    return !!name && /^[a-zA-Z0-9\s]*$/.test(name);
  }

  isValidCode(code: string): boolean {
    return !!code && /^[a-zA-Z0-9]*$/.test(code);
  }

  isValidDescription(description: string): boolean {
    return /^[a-zA-Z0-9\s]*$/.test(description);
  }

  isValidForm(): boolean {
    return this.isValidName(this.formData.name) && this.isValidCode(this.formData.categoryCode) && this.isValidDescription(this.formData.description);
  }
}
