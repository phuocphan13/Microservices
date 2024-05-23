import { Component } from '@angular/core';

@Component({
  selector: 'app-product-admin',
  templateUrl: './product-admin.component.html',
  styleUrl: './product-admin.component.css'
})
export class ProductAdminComponent {

  data = [
    { column1: '1', column2: 'Data Name', column3: 'Data Code', column4: 'A mobile phone is a wireless handheld device that allows users to make and receive calls. While the earliest generation of mobile phones could only make and receive calls, todays mobile phones do a lot more, accommodating web browsers, games, cameras, video players and navigational systems.' },
    { column1: '2', column2: 'Data Name', column3: 'Data Code', column4: 'The first mobile phones, as mentioned, were only used to make and receive calls, and they were so bulky it was impossible to carry them in a pocket. These phones used primitive RFID and wireless systems to carry signals from a cabled PSTN endpoint.' },
    { column1: '3', column2: 'Data Name', column3: 'Data Code', column4: 'Later, mobile phones belonging to the Global System for Mobile Communications (GSM) network became capable of sending and receiving text messages. As these devices evolved, they became smaller and more features were added, such as multimedia messaging service (MMS), which allowed users to send and receive images..' },
    { column1: '4', column2: 'Data Name', column3: 'Data Code', column4: 'Most of these MMS-capable devices were also equipped with cameras, which allowed users to capture photos, add captions, and send them to friends and relatives who also had MMS-capable phones.' },
    { column1: '5', column2: 'Data Name', column3: 'Data Code', column4: 'Along with the texting and camera features, cell phones started to be made with a limited capability to access the Internet, known as “data services.” The earliest phone browsers were proprietary and only allowed for the use of a small subsection of the Internet, allowing users to access items like weather, news, and sports updates.' },
  ];

  onClickAction(actionType: string) {
    switch (actionType) {
      case 'view':
        break;
      case 'edit':
        break;
      case 'delete':
        break;
      default:
        break;
    }
  }
}
