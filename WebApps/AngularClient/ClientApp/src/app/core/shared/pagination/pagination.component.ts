import { Component, Input, Output, EventEmitter } from '@angular/core';


@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent {

  @Input() totalItems: number = 0; // tổng items
  @Input() itemsPerPage: number = 3; // số items mỗi trang
  @Input() currentPage: number = 1; // trang hiện tại đang xem
  @Output() currentPageChange = new EventEmitter<number>(); // Sự kiện thay đổi trang
  @Output() pageChanged = new EventEmitter<number>(); //đổi trang khác

  get totalPages():number{
    return Math.ceil(this.totalItems / this.itemsPerPage); //tính số lượng trang hiển thị
  }

  moveToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) 
    {
      this.currentPage = page;
      this.currentPageChange.emit(this.currentPage); // Emit giá trị mới cho two-way binding
      this.pageChanged.emit(this.currentPage);
    }
  }

  nextPage(): void {
    this.moveToPage(this.currentPage + 1);
  }

  previousPage(): void {
    this.moveToPage(this.currentPage - 1);
  }

  firstPage(): boolean {
    return this.currentPage === 1;
  }

  lastPage(): boolean {
    return this.currentPage === this.totalPages;
  }
}
