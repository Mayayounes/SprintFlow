import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-main-component',
  imports: [CommonModule , RouterOutlet],
  templateUrl: './main-component.html',
  styleUrl: './main-component.css',
})
export class MainComponent {

  email = localStorage.getItem('email');
  role = localStorage.getItem('role');

  constructor(private router: Router) {}

  logout() {
    localStorage.clear();
    this.router.navigate(['/auth']);
  }
}
