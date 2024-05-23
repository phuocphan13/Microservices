import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  showCatalogAdminButtons = false;

  constructor(private router: Router) {}

  navigateTo(page: string) {
    this.router.navigate([page]);
  }
  
  onClickCatalogAdmin(): void {
    this.showCatalogAdminButtons = !this.showCatalogAdminButtons;
  }
}
