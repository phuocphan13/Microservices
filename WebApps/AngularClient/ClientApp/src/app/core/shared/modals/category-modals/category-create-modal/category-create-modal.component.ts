import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-category-create-modal',
  templateUrl: './category-create-modal.component.html',
  styleUrls: ['./category-create-modal.component.css']
})
export class CategoryCreateModalComponent {

  createForm: FormGroup;

  constructor(private formBuilder: FormBuilder, public activeModal: NgbActiveModal) {
    this.createForm = this.formBuilder.group({
      name: ['', Validators.required],
      code: ['', Validators.required],
      description: ['']
    });
  }

  onSubmit(){
    //call API
    this.activeModal.close();
  }
  onClickClose() {
    this.activeModal.close();
  }
}
