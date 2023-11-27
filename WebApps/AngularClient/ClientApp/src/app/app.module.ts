import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { CatalogComponent } from './catalog/catalog.component';
import { DetailsComponent } from './catalog/details/details.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    CatalogComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { 
        path: '', component: HomeComponent
      },
      {
        path:'home', component:HomeComponent
      },
      { 
        path: 'counter', component: CounterComponent 
      },
      { 
        path: 'fetch-data', component: FetchDataComponent 
      },
      {
        path: 'catalog', component:CatalogComponent,children:[
          {
            path: 'details', component:DetailsComponent
          }
        ]
      }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
