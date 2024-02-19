import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CategoryService } from 'src/app/core/service/catalog/category.service';

@Component({
  selector: 'app-category-delete-modal',
  templateUrl: './category-delete-modal.component.html',
  styleUrls: ['./category-delete-modal.component.css']
})
export class CategoryDeleteModalComponent {

  @Input() modalData: any;

  constructor(public activeModal: NgbActiveModal, private categoryService: CategoryService) {
  }

  async onClickDelete(){
    if (this.modalData) {
      await this.categoryService.deleteCategoryAsync(this.modalData.id);
      this.activeModal.close(this.modalData.id);
    }
      else{
        // báo lỗi
      }
  }

  onClickClose() {
    this.activeModal.close();
  }
}
