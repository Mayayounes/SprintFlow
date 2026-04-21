import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ToastService } from '../../../core/services/toast/toast';

@Component({
  selector: 'app-toast',
  imports: [CommonModule],
  standalone: true,
  templateUrl: './toast.html',
  styleUrl: './toast.css',
})
export class Toast {
  toast$;

  constructor(private toastService: ToastService) {
    this.toast$ = this.toastService.toast$;
  }
}
