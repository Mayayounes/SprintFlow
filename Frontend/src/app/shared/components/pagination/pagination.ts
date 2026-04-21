import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  imports: [CommonModule],
  templateUrl: './pagination.html',
  styleUrl: './pagination.css',
})
export class Pagination {
  @Input() pageNumber!: number;
  @Input() totalPages!: number;

  @Output() pageChange = new EventEmitter<number>();

  next() {
    if (this.pageNumber < this.totalPages) {
      this.pageChange.emit(this.pageNumber + 1);
    }
  }

  prev() {
    if (this.pageNumber > 1) {
      this.pageChange.emit(this.pageNumber - 1);
    }
  }

}
