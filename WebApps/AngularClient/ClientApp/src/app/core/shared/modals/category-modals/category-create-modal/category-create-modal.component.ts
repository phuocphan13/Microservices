import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-category-create-modal',
  templateUrl: './category-create-modal.component.html',
  styleUrls: ['./category-create-modal.component.css']
})
export class CategoryCreateModalComponent {

  name: string = '';
  code: string = '';
  description: string = '';
  
  constructor( public activeModal: NgbActiveModal) {
   
  }

  onSubmit(){
    //call API
    this.activeModal.close();
  }
  onClickClose() {
    this.activeModal.close();
  }
}
