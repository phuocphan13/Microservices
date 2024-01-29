import { Component } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { CategorySummary } from 'src/app/core/models/catalog/category-models/category-summary.model';
import { CategoryService } from 'src/app/core/service/catalog/category.service';
import { CategoryCreateModalComponent } from 'src/app/core/shared/modals/category-modals/category-create-modal/category-create-modal.component';
import { CategoryDeleteModalComponent } from 'src/app/core/shared/modals/category-modals/category-delete-modal/category-delete-modal.component';
import { CategoryUpdateModalComponent } from 'src/app/core/shared/modals/category-modals/category-update-modal/category-update-modal.component';
import { CategoryViewModalComponent } from 'src/app/core/shared/modals/category-modals/category-view-modal/category-view-modal.component';

@Component({
  selector: 'app-category-admin',
  templateUrl: './category-admin.component.html',
  styleUrls: ['./category-admin.component.css']
})
export class CategoryAdminComponent {

  categories: CategorySummary[] = [];

  constructor(private modalService: NgbModal, private categoryService: CategoryService) {
  }

  ngOnInit() {
    this.getCategoriesAsync();
  }

  async getCategoriesAsync() {
    this.categories = await this.categoryService.getCategoriesAsync();
  }

  onClickCreate(){
    let ngbModalOptions: NgbModalOptions = {
      backdrop: 'static',
      keyboard: false,
    };

    let modal = this.modalService.open(CategoryCreateModalComponent, ngbModalOptions);

    modal.componentInstance.isView = false;
  }

  onClickAction(actionType: string, rowData: any) {
    switch (actionType) {
      case 'view': {
        let ngbModalOptions: NgbModalOptions = {
          backdrop: 'static',
          keyboard: false,
        };
  
        let modal = this.modalService.open(CategoryViewModalComponent, ngbModalOptions);
  
        modal.componentInstance.modalData = rowData;
        modal.componentInstance.isView = true;

        break;
      }
      case 'edit': {
        let ngbModalOptions: NgbModalOptions = {
          backdrop: 'static',
          keyboard: false,
        };
  
        let modal = this.modalService.open(CategoryUpdateModalComponent, ngbModalOptions);
  
        modal.componentInstance.modalData = { ...rowData };
        modal.componentInstance.isView = false;
  
        break;
      }
      case 'delete':{
        let ngbModalOptions: NgbModalOptions = {
          backdrop: 'static',
          keyboard: false,
        };
  
        let modal = this.modalService.open(CategoryDeleteModalComponent, ngbModalOptions);

        modal.componentInstance.modalData = { ...rowData };

        break;
      }
      default:
        break;
    }
  }
}