import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { Action } from "../../../../../common/const";
import { CategoryService } from "../../../../service/catalog/category.service";
import { CategorySummary } from 'src/app/core/models/catalog/category-models/category-summary.model';

@Component({
  selector: 'app-category-modal',
  templateUrl: './category-modal.component.html',
  styleUrls: ['./category-modal.component.css']
})
export class CategoryModalComponent implements OnInit {
  @Input() modalData: any;
  @Input() type: string = Action.View;
  title: string ="";
  isDisabled: boolean = false;
  formData: any = {};
  @Output() successEvent = new EventEmitter<boolean>();

  constructor(public activeModal: NgbActiveModal, private categoryService: CategoryService) {}

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
    let result;
    if (this.isValidForm())
    {if (this.type === Action.Edit || this.type === Action.Create) {
        this.successEvent.emit(this.formData);
      }
    // if (this.type === Action.Edit) {
    //   this.categoryService.updateCategoryAsync(this.formData).then(result => this.handleResponse(result));
    // } else if (this.type == Action.Create) {
    //   this.categoryService.createCategoryAsync(this.formData).then(result => this.handleResponse(result));
    // }

    if (result) {
      this.handleResponse(result);
      this.successEvent.emit(true); // Load lại danh sách nếu thành công
    }
  }
  }

  private handleResponse(response: any) {
    if (response) {
      this.activeModal.close();
    } else {
      // Báo lỗi
    }
  }

  onClickClose() {
    this.activeModal.close();
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
