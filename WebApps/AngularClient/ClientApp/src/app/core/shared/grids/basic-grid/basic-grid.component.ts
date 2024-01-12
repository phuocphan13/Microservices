import {Component, Input} from '@angular/core';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-basic-grid',
  templateUrl: './basic-grid.component.html',
  styleUrl: './basic-grid.component.css'
})
export class BasicGridComponent {
  @Input() listHeaders: any[] = []
  @Input() listData: any[] = [];

  datas = [
    {
      id: 1,
      name: "a"
    },
    {
      id: 2,
      name: "b"
    }]
}
