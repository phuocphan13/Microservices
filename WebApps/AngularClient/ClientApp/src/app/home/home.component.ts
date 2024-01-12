import {Component} from '@angular/core';
import {NgbModal, NgbModalOptions} from "@ng-bootstrap/ng-bootstrap";
import {ProductModalComponent} from "../core/shared/modals/product-modal/product-modal.component";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  constructor(private modalService: NgbModal) {
  }

  onClickOpenModal() {
    let ngbModalOptions: NgbModalOptions = {
      backdrop: 'static',
      keyboard: false,
    };

    let modal = this.modalService.open(ProductModalComponent, ngbModalOptions)
    modal.componentInstance.isView = true;

    modal.result.then((result: any) => {
      if (result) {
        //call API
      }
    })
  }
}
