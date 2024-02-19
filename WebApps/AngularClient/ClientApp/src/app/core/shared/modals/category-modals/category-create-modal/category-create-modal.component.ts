import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CategoryService } from 'src/app/core/service/catalog/category.service';

@Component({
  selector: 'app-category-create-modal',
  templateUrl: './category-create-modal.component.html',
  styleUrls: ['./category-create-modal.component.css']
})
export class CategoryCreateModalComponent {

  createForm: FormGroup;
  
  constructor( public activeModal: NgbActiveModal, private formBuilder: FormBuilder, private categoryService: CategoryService) {
    this.createForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      categoryCode: ['', [Validators.required]],
      description: ['']
    });
  }

  async onSubmit(){
    if(this.createForm.valid){
      const categoryDetail = this.createForm.value;
      await this.categoryService.createCategoryAsync(categoryDetail);
      this.activeModal.close();
    }
    else{
      //error message
    }
  }
  onClickClose() {
    this.activeModal.close();
  }
}
