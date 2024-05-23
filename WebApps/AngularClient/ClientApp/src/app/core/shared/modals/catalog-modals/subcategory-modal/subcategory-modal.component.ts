import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { Action } from "../../../../../common/const";

@Component({
  selector: 'app-subcategory-modal',
  templateUrl: './subcategory-modal.component.html',
  styleUrls: ['./subcategory-modal.component.css']
})
export class SubcategoryModalComponent implements OnInit {
  [x: string]: any;
  @Input() modalData: any;
  @Input() type: string = Action.View;
  @Output() successEvent = new EventEmitter<boolean>();

  title: string ="";
  isDisabled: boolean = false;

  formData: any = {};
  tempFormData: any = {};

  constructor(public activeModal: NgbActiveModal) {}

  ngOnInit(): void {
    this.setTitle();
    this.setDisabledState();
    this.tempFormData = { ...this.formData };
  }

  setTitle() {
    if (this.type == 'edit') {
      this.title = 'Edit SubCategory';
    } else if (this.type == 'create') {
      this.title = 'Create New SubCategory';
    } else if (this.type == 'view') {
      this.title = 'View SubCategory';
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
}
