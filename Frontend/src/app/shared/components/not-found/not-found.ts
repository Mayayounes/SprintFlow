import { Component } from '@angular/core';
import { Auth } from '../../../core/services/Auth/auth';
import { Router } from '@angular/router';
import { ROLE_ROUTES } from '../../../core/guards/auth.util';

@Component({
  selector: 'app-not-found',
  imports: [],
  templateUrl: './not-found.html',
  styleUrl: './not-found.css',
})
export class NotFound {

  constructor(private auth: Auth , private router : Router){}

  goHome() {

  const role = this.auth.getRole()
  const targetRoute = ROLE_ROUTES[role as string];
  this.router.navigateByUrl(targetRoute  || '/');
}
}
