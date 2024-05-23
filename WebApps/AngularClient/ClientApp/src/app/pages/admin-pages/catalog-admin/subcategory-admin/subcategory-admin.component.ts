import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ProductModalComponent } from 'src/app/core/shared/modals/catalog-modals/product-modal/product-modal.component';
import { Action } from 'src/app/common/const';
import { ConfirmationModalComponent } from 'src/app/core/shared/modals/common/confirmation-modal/confirmation-modal.component';
import { SubcategoryModalComponent } from 'src/app/core/shared/modals/catalog-modals/subcategory-modal/subcategory-modal.component';
import { SubCategorySummary } from 'src/app/core/models/catalog/subcategory-models/subcategory-summary.model';
import { SubCategoryService } from 'src/app/core/service/catalog/subcategory.service';

@Component({
  selector: 'app-subcategory-admin',
  templateUrl: './subcategory-admin.component.html',
  styleUrl: './subcategory-admin.component.css'
})

export class SubcategoryAdminComponent implements OnInit {

  subCategories: SubCategorySummary[] = [];
  ngbModalOptions: NgbModalOptions = {
    backdrop: 'static',
    keyboard: false,
  };

  constructor(private modalService: NgbModal, private subCategoryService: SubCategoryService) {
  }

  async ngOnInit() {
    await this.getSubCategoryAsync();
  }
  private async getSubCategoryAsync() {
    this.subCategories = await this.subCategoryService.getSubCategoryAsync();
  }

  modifyAction(actionType: string, rowData: any) {
    let modal = this.modalService.open(SubcategoryModalComponent, this.ngbModalOptions);
    modal.componentInstance.formData = rowData;
    modal.componentInstance.type = actionType;
  }

  onClickOpenModal() {
    let modal = this.modalService.open(ProductModalComponent, this.ngbModalOptions)
    modal.componentInstance.isView = true;

  }

  onClickAction(actionType: string, rowData: any) {
    if (actionType == Action.Delete) {
      let modal = this.modalService.open(ConfirmationModalComponent, this.ngbModalOptions);
      modal.componentInstance.title = 'Delete Category';
      modal.componentInstance.name = 'category';
    } else {
      this.modifyAction(actionType, rowData);
    }
  }

  onClickCreate(actionType:string){
    let modal = this.modalService.open(SubcategoryModalComponent, this.ngbModalOptions);
    modal.componentInstance.type = actionType;
    modal.componentInstance.successEvent.subscribe(async (fromData: any) => {
      if(actionType === Action.Create) {
        await this.subCategoryService.creatSubCategoryAsync(fromData);
        await this.getSubCategoryAsync();
      }
    });
  }
}
