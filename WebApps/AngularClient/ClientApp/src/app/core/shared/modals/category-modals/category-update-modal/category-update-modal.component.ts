import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CategoryService } from 'src/app/core/service/catalog/category.service';

@Component({
  selector: 'app-category-update-modal',
  templateUrl: './category-update-modal.component.html',
  styleUrls: ['./category-update-modal.component.css']
})
export class CategoryUpdateModalComponent {
  
  @Input() modalData: any;

  constructor(public activeModal: NgbActiveModal,private categoryService: CategoryService) {
  }

  async onSubmit() {
    if (this.modalData) {
      await this.categoryService.updateCategoryAsync(this.modalData);
      this.activeModal.close();
    } else {
      // báo lỗi
    }
  }

  onClickClose() {
    this.activeModal.close();
  }
}
