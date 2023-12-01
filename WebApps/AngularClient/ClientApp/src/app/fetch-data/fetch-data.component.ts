import {Component, Inject, OnInit} from '@angular/core';
import {CatalogService} from "../core/service/catalog.service";

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent implements OnInit {
  constructor(private catalogService: CatalogService) {
  }

  async getProductsAsync() {
    let products = await this.catalogService.getProductByCategoryAsync("Smart Phone");
    console.log(products);
  }

  async ngOnInit() {
    await this.getProductsAsync();
  }
}

