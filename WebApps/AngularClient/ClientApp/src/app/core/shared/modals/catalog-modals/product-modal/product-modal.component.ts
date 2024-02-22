import {Component, EventEmitter} from '@angular/core';
import {CommonModule} from '@angular/common';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {ProductDetail} from "../../../models/catalog/product-models/product-detail.model";

@Component({
  selector: 'app-product-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-modal.component.html',
  styleUrl: './product-modal.component.css'
})
export class ProductModalComponent {
  isView: boolean = false;
  product: ProductDetail = new ProductDetail();

  constructor(public activeModal: NgbActiveModal) {
  }

  onClickSave() {
    this.activeModal.close(this.product);
  }

  onClickClose() {
    this.activeModal.close();
  }
}
