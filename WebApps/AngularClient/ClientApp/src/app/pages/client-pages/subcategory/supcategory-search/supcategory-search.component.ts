import { Component } from '@angular/core';
import { SubCategorySummary } from 'src/app/core/models/catalog/subcategory-models/subcategory-summary.model';
import { SubCategoryService } from 'src/app/core/service/catalog/subcategory.service'

@Component({
  selector: 'app-supcategory-search',
  standalone: true,
  imports: [],
  templateUrl: './supcategory-search.component.html',
  styleUrl: './supcategory-search.component.css'
})
export class SupcategorySearchComponent {

  constructor(private subCategoryService: SubCategoryService) {
  }

  async onclickSearchButton(searchKey: string) : Promise<SubCategorySummary[]> {
    const key = searchKey.trim();
    return key ? await this.subCategoryService.getSubCategoryByCatagoryIdAsync(key) : await this.subCategoryService.getSubCategoryAsync();
  }
}
