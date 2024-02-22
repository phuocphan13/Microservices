import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from './home/home.component';
import {CounterComponent} from './counter/counter.component';
import {FetchDataComponent} from './fetch-data/fetch-data.component';
import {CatalogComponent} from './catalog/catalog.component';
import {ProductListComponent} from './catalog/product-list/product-list.component';
import {ProductComponent} from './catalog/product/product.component';

const appRouter: Routes = [
  {
    path: '', redirectTo: '/home', pathMatch: 'full'
  },
  {
    path: 'home', component: HomeComponent
  },
  {
    path: 'counter', component: CounterComponent
  },
  {
    path: 'fetch-data', component: FetchDataComponent
  },
  {
    path: 'catalog', component: CatalogComponent,
    children: [
      {
        path: 'product-list', component: ProductListComponent
      },
      {
        path: 'product-detail/:id', component: ProductComponent
      }
    ]
  },
  {
    path: '**', component: HomeComponent
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRouter),
  ],
  exports: [
    RouterModule,
  ],
})
export class AppRoutingModule {
}
