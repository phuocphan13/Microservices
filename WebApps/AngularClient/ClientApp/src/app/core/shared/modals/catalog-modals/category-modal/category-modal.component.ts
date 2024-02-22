import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { Action } from "../../../../../common/const";
import { CategoryService } from "../../../../service/catalog/category.service";

@Component({
  selector: 'app-category-modal',
  templateUrl: './category-modal.component.html',
  styleUrls: ['./category-modal.component.css']
})
export class CategoryModalComponent implements OnInit {
  createForm: FormGroup;
  @Input() modalData: any;
  @Input() type: string = Action.View;
  isView: boolean = false;
  isEdit: boolean = false; //Incase of Edit for not allow to edit Name

  constructor(public activeModal: NgbActiveModal, private formBuilder: FormBuilder, private categoryService: CategoryService) {
    this.createForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      categoryCode: ['', [Validators.required]],
      description: ['']
    });

    this.isView = this.type === Action.View;
    this.isEdit = this.type === Action.Edit;
  }

  ngOnInit(): void {
  }

  async onSubmit() {
    if (this.type === Action.Edit) {
      //Todo: handle validation with formGroup
      if (this.createForm.valid) {
        let result = await this.categoryService.updateCategoryAsync(this.modalData);
        this.handleResponse(result);
      }
    } else if (this.type == Action.Create) {
      //Todo: handle validation with formGroup
      if (this.createForm.valid) {
        const categoryDetail = this.createForm.value;
        let result = await this.categoryService.createCategoryAsync(categoryDetail);
        this.handleResponse(result);
      }
    }
  }

  private handleResponse(response: any) {
    if (response) {
      this.activeModal.close();
    } else {
      // báo lỗi
    }
  }

  onClickClose() {
    this.activeModal.close();
  }
}
