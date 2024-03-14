import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { CategorySummary } from 'src/app/core/models/catalog/category-models/category-summary.model';
import { CategoryService } from 'src/app/core/service/catalog/category.service';
import { Action } from "../../../../common/const";
import { CategoryModalComponent } from "../../../../core/shared/modals/catalog-modals/category-modal/category-modal.component";
import { ConfirmationModalComponent } from "../../../../core/shared/modals/common/confirmation-modal/confirmation-modal.component";

@Component({
  selector: 'app-category-admin',
  templateUrl: './category-admin.component.html',
  styleUrls: ['./category-admin.component.css']
})
export class CategoryAdminComponent implements OnInit {

  categories: CategorySummary[] = [];
  ngbModalOptions: NgbModalOptions = {
    backdrop: 'static',
    keyboard: false,
  };

  constructor(private modalService: NgbModal, private categoryService: CategoryService) {
  }

  async ngOnInit() {
    await this.getCategoriesAsync();
  }

  private async getCategoriesAsync() {
    this.categories = await this.categoryService.getCategoriesAsync();
  }

  modifyAction(actionType: string, rowData: any) {
    let isView = false;

    if (actionType === Action.View) {
      isView = true;
    }

    let modal = this.modalService.open(CategoryModalComponent, this.ngbModalOptions);
    modal.componentInstance.formData = rowData;
    modal.componentInstance.type = actionType;
    modal.componentInstance.successEvent.subscribe((formData: any) => {
      if (actionType === Action.Edit) {
        this.categoryService.updateCategoryAsync(formData).then(result => {
          if (result) {
            this.getCategoriesAsync();
          }
        });
      } else if (actionType === Action.Create) {
        this.categoryService.createCategoryAsync(formData).then(result => {
          if (result) {
            this.getCategoriesAsync();
          }
        });
      }
    });
  }

  onClickAction(actionType: string, rowData: any) {
    if (actionType == Action.Delete) {
      let modal = this.modalService.open(ConfirmationModalComponent, this.ngbModalOptions);
      modal.componentInstance.title = 'Delete Category';
      modal.componentInstance.name = 'category';

      modal.result.then(async (result) => {
        if (result) {
          let result = await this.categoryService.deleteCategoryAsync(rowData.id);
          if (result) {
            await this.getCategoriesAsync();
          }
        }
      });

    } else {
      this.modifyAction(actionType, rowData);
    }
  }

  onClickCreate(actionType:string){
    let modal = this.modalService.open(CategoryModalComponent, this.ngbModalOptions);
    modal.componentInstance.type = actionType;
    modal.componentInstance.successEvent.subscribe((success: boolean) => {
      if (success) {
        this.getCategoriesAsync();
      }
    });
  }
}
