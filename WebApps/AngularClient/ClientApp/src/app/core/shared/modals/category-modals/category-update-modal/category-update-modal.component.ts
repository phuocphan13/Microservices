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
  updateForm = this.formBuilder.group({
    name: ['', [Validators.required]],
    code: ['', [Validators.required]],
    description: ['']
  });

  constructor(public activeModal: NgbActiveModal,private categoryService: CategoryService, private formBuilder: FormBuilder) {
    this.updateForm.patchValue(this.modalData);
  }

  async onSubmit(){
    if (this.updateForm.valid) {
      const categoryDetail = this.updateForm.value;
      await this.categoryService.updateCategoryAsync(categoryDetail);
      this.activeModal.close();
    } else {
      //error message
    }
  }
  onClickClose() {
    this.activeModal.close();
  }
}
