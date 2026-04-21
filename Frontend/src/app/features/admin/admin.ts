import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-admin',
  imports: [CommonModule, RouterOutlet , RouterModule],
  standalone: true,
  templateUrl: './admin.html',
  styleUrl: './admin.css',
})
export class Admin {
}
