import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface ErrorModalState {
  message: string[];
  visible: boolean;
}

@Injectable({ providedIn: 'root' })
export class ErrorModalService {

  private stateSubject = new BehaviorSubject<ErrorModalState | null>(null);
  state$ = this.stateSubject.asObservable();

  show(message: string | string[]) {
    const normalized = Array.isArray(message) ? message : [message];

    this.stateSubject.next({
      message: normalized,
      visible: true
  });
  }

  close() {
    this.stateSubject.next(null);
  }
}
