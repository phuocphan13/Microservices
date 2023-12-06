import {Component, Inject, OnInit} from '@angular/core';
import {CatalogService} from "../core/service/catalog.service";
import { ProductDetail } from '../core/models/ProductDetail.model';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent implements OnInit {


  productDetail1:ProductDetail = new ProductDetail();
  productDetail2:ProductDetail = new ProductDetail();

  constructor(private catalogService: CatalogService) {
    this.productDetail1 = {
      id: '602d2149e773f2a3990b47f5',
      name: 'IPhone X',
      category: 'Smart Phone',
      summary: 'This phone is the company`s biggest change to its flagship smartphone in years. It includes a borderless.',
      description: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.',
      subCategory:'iphone',
      imageFile: 'product-1.png',
      price: 950.00
    };

    this.productDetail2 = {
      id: '602d2149e773f2a3990b47f5',
      name: 'IPhone X',
      category: 'Smart Phone',
      summary: 'This phone is the company`s biggest change to its flagship smartphone in years. It includes a borderless.',
      description: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.',
      subCategory:'iphone',
      imageFile: 'product-1.png',
      price: 950.00
    };

  }

  


  async getProductsAsync() {
    let products = await this.catalogService.getProductsAsync;
    console.log(products);
  }
  
  async getProductByIdAsync() {
    let product = await this.catalogService.getProductByIdAsync("655b766bd61059921a53744d");
  }

  async getProductByCategoryAsync() {
    let product = await this.catalogService.getProductByCategoryAsync("Smart Phone");
  }

  async createProduct() {
    let product = await this.catalogService.createProductAsync(this.productDetail1);
  }

  async deleteProduct(){
    let product = await this.catalogService.deleteProductAsync("602d2149e773f2a3990b47f5")
  }

  async updateProduct() {
    let product = await this.catalogService.updateProductAsync(this.productDetail2);
  }

  async ngOnInit() {
    await this.getProductsAsync();
  }
}

