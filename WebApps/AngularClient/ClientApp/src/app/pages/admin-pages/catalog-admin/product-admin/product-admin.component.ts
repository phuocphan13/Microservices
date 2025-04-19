import { Component, OnInit } from '@angular/core';
import { ProductSummary } from 'src/app/core/models/catalog/product-models/product-summary.model';
import { CatalogService } from 'src/app/core/service/catalog/catalog.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-product-admin',
  templateUrl: './product-admin.component.html',
  styleUrl: './product-admin.component.css'
})
export class ProductAdminComponent implements OnInit {
  products: ProductSummary[] = [];
  allProducts: ProductSummary[] = [];
  currentPage: number = 1;
  totalItems: number = 0;
  start: number = 0;
  length: number = 3;

  constructor(private modalService: NgbModal, private catalogService: CatalogService) {
  }

  async ngOnInit() {
    await this.GetProductsPagingAsync(this.start, this.length);
    
    await this.CountProduct();
  }

  private async GetProductsAsync () {
    this.products = await this.catalogService.getProductsAsync();
  }

  private async CountProduct () {
    this.allProducts = await this.catalogService.getProductsAsync();
    this.totalItems = this.allProducts.length;
  }

  private async GetProductsPagingAsync (start: number, length: number ) {
    this.products = await this.catalogService.getProductsPagingAsync(start, length);
  }

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

  async onPageChanged(page: number): Promise<void> { 
    if(page === 1)
    {
      this.start = 0;
    }
    else
    {
      this.start = (page - 1) * this.length;
    }

    this.currentPage = page;
    //gọi tới Product paging truyền vào tham số Start và Length
    await this.GetProductsPagingAsync(this.start, this.length);
  }
}
