import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { Auth } from '../../../core/services/Auth/auth';
import { ROLE_ROUTES } from '../../../core/guards/auth.util';

@Component({
  selector: 'app-dashboard',
  imports: [RouterModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit{
constructor(private router: Router , private auth : Auth) {}

ngOnInit() {
  const isLoggedIn = this.auth.isLoggedIn();
  const role = localStorage.getItem('role');

  if (isLoggedIn) {
    const targetRoute = ROLE_ROUTES[role as string]
      this.router.navigateByUrl(targetRoute || '/', { replaceUrl: true });
    }
  }
}
