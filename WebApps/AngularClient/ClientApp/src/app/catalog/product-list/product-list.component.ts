import {Component, OnInit} from '@angular/core';
import {CatalogService} from 'src/app/core/service/catalog.service';
import {ProductSummary} from "../../core/models/catalog/product-models/product-summary.model";

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {

  products: ProductSummary[] = [];

  constructor(private catalogService: CatalogService) {
  }

  ngOnInit() {
    this.getProductsAsync();
  }

  async getProductsAsync() {
    this.products = await this.catalogService.getProductsAsync();
  }
}
