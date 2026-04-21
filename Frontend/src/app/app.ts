import { Component, signal } from '@angular/core';
import { RouterOutlet, RouterModule } from '@angular/router';
import { Toast } from './shared/components/toast/toast';
import { ErrorModalComponent } from './shared/components/error-modal/error-modal';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet , Toast , ErrorModalComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('sprintFlow-Fontend');
}
