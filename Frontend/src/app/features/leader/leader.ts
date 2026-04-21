import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-leader',
  imports: [RouterOutlet, CommonModule , RouterModule],
  templateUrl: './leader.html',
  styleUrl: './leader.css',
})
export class Leader{
}
