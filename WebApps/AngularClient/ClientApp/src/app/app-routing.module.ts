import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from './home/home.component';
import {CatalogComponent} from "./pages/client-pages/catalog/catalog.component";
import {ProductListComponent} from "./pages/client-pages/catalog/product-list/product-list.component";
import {ProductComponent} from "./pages/client-pages/catalog/product/product.component";
import {CatalogAdminComponent} from "./pages/admin-pages/catalog-admin/catalog-admin.component";
import {ProductAdminComponent} from "./pages/admin-pages/catalog-admin/product-admin/product-admin.component";
import {CategoryAdminComponent} from "./pages/admin-pages/catalog-admin/category-admin/category-admin.component";
import { LogInComponent } from "./pages/identity-pages/log-in/log-in.component";

const appRouter: Routes = [
  {
    path: '', redirectTo: '/home', pathMatch: 'full'
  },
  {
    path: 'home', component: HomeComponent
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
    path: 'catalog-admin', component: CatalogAdminComponent,
    children: [
      {
        path: 'product-admin', component: ProductAdminComponent,
      },
      {
        path: 'category-admin', component: CategoryAdminComponent,
      }
    ]
  },
  {
    path: 'log-in', component: LogInComponent,
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
