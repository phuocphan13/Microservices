import { Component, NgModule, OnInit } from '@angular/core';
import { SubCategorySummary } from 'src/app/core/models/catalog/subcategory-models/subcategory-summary.model';
import { SubCategoryService } from 'src/app/core/service/catalog/subcategory.service'
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-supcategory-search',
  templateUrl: './supcategory-search.component.html',
  styleUrl: './supcategory-search.component.css'
})

export class SupcategorySearchComponent implements OnInit {
  subCategories: SubCategorySummary[] = [];
  searchKey: any = "";

  ngbModalOptions: NgbModalOptions = {
    backdrop: 'static',
    keyboard: false,
  };

  constructor(private subCategoryService: SubCategoryService) {
  }

  async ngOnInit() {
     await this.onclickSearchButton();
  }

  private async getSubCategoriesAsync() {
    this.subCategories = await this.subCategoryService.getSubCategoryAsync();
  }

  private async getSubCategoryByIdAsync() {
    this.subCategories = [];
    let data = await this.subCategoryService.getSubCategoryByIdAsync(this.searchKey);
    this.subCategories.push(data)
  }

  private async getSubCategoryByNameAsync() {
    this.subCategories = await this.subCategoryService.getSubCategoryByNameAsync(this.searchKey);
  }

  async onclickSearchButton() {
    if(this.searchKey == "")
      {
        await this.getSubCategoriesAsync();
      }
    else if (this.searchKey != "")
      {    
        await this.getSubCategoryByIdAsync();
      }
  }
}
