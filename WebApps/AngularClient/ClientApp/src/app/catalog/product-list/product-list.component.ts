import { Component } from '@angular/core';
import { ProductSummary } from 'src/app/core/models/product-sumary.model';
import { CatalogService } from 'src/app/core/service/catalog.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent {

  products:ProductSummary[] = [];

  constructor(private catalogService: CatalogService){}
  
  ngOnInit(){
    this.getProductsAsync();
  }

  async getProductsAsync() {
    this.products = await this.catalogService.getProductsAsync();
  }
}
