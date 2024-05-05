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
  @Output() successEvent = new EventEmitter<boolean>();

  title: string ="";
  isDisabled: boolean = false;

  formData: any = {};
  tempFormData: any = {};
  isDirty: Record<string, boolean> = {
    name: false,
    categoryCode: false,
    description: false
  };

  constructor(public activeModal: NgbActiveModal) {}

  ngOnInit(): void {
    this.setTitle();
    this.setDisabledState();
    this.tempFormData = { ...this.formData };
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
    let result;
    if (this.type === Action.Edit || this.type === Action.Create) {
        this.successEvent.emit(this.tempFormData);
        this.activeModal.close();
    }

    if (result) {
      this.handleResponse(result);
      this.successEvent.emit(true);
    }
  }

  onClickClose() {
    this.activeModal.close();
    this.closeEvent.emit();
  }

  setDisabledState() {
    this.isDisabled = this.type === 'view';
  }

  markAsDirty(fieldName: string) {
    this.isDirty[fieldName] = true;
  }

  isValidField(fieldName: string): boolean {
    const value = this.tempFormData[fieldName];
    switch (fieldName) {
      case 'name':
        return this.isValidName(value);
      case 'categoryCode':
        return this.isValidCode(value);
      case 'description':
        return this.isValidDescription(value);
      default:
        return false;
    }
  }

  isValidName(name: string): boolean {
    return !!name && /^[a-zA-Z0-9\s]*$/.test(name);
  }

  isValidCode(code: string): boolean {
    return !!code && /^[a-zA-Z0-9]*$/.test(code);
  }

  isValidDescription(description: string): boolean {
    return !/[@#$%^&*()_+|~=`{}\[\]:";'<>?,.\/]/.test(description);
  }
}
