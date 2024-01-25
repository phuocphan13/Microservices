import { Component } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
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

  constructor(private modalService: NgbModal) {
    
  }

  data = [
    { id: '1', name: 'Data Name', code: 'Data Code', description: 'A mobile phone is a wireless handheld device that allows users to make and receive calls. While the earliest generation of mobile phones could only make and receive calls, todays mobile phones do a lot more, accommodating web browsers, games, cameras, video players and navigational systems.' },
    { id: '2', name: 'Data Name', code: 'Data Code', description: 'The first mobile phones, as mentioned, were only used to make and receive calls, and they were so bulky it was impossible to carry them in a pocket. These phones used primitive RFID and wireless systems to carry signals from a cabled PSTN endpoint.' },
    { id: '3', name: 'Data Name', code: 'Data Code', description: 'Later, mobile phones belonging to the Global System for Mobile Communications (GSM) network became capable of sending and receiving text messages. As these devices evolved, they became smaller and more features were added, such as multimedia messaging service (MMS), which allowed users to send and receive images..' },
    { id: '4', name: 'Data Name', code: 'Data Code', description: 'Most of these MMS-capable devices were also equipped with cameras, which allowed users to capture photos, add captions, and send them to friends and relatives who also had MMS-capable phones.' },
    { id: '5', name: 'Data Name', code: 'Data Code', description: 'Along with the texting and camera features, cell phones started to be made with a limited capability to access the Internet, known as “data services.” The earliest phone browsers were proprietary and only allowed for the use of a small subsection of the Internet, allowing users to access items like weather, news, and sports updates.' },
  ];

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
      case 'update': {
        let ngbModalOptions: NgbModalOptions = {
          backdrop: 'static',
          keyboard: false,
        };
  
        let modal = this.modalService.open(CategoryUpdateModalComponent, ngbModalOptions);
  
        modal.componentInstance.modalData = { name: rowData.name,
          code: rowData.code,
          description: rowData.description };
        modal.componentInstance.isView = false;
  
        break;
      }
      case 'delete':{
        let ngbModalOptions: NgbModalOptions = {
          backdrop: 'static',
          keyboard: false,
        };
  
        this.modalService.open(CategoryDeleteModalComponent, ngbModalOptions);

        break;
      }
      default:
        break;
    }
  }
}
