import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ErrorModalService } from '../../../core/services/error-modal/error-modal';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-error-modal',
  standalone: true,
  imports: [CommonModule , FormsModule],
  templateUrl: './error-modal.html',
  styleUrl: './error-modal.css'
})

export class ErrorModalComponent {

  state$;

  constructor(private errorModalService: ErrorModalService) {
    this.state$ = this.errorModalService.state$;
  }
  close() {
    this.errorModalService.close();
  }

  isArray(value: any): boolean {
    return Array.isArray(value);
  }
}
