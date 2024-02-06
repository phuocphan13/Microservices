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

  onClickDelete(){
    this.categoryService.deleteCategoryAsync(this.modalData.id).then(() => {
         this.activeModal.close(this.modalData.id);
       });
      this.activeModal.close(this.modalData.id);
  }
  onClickClose() {
    this.activeModal.close();
  }
}
