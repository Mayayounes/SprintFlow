import { TestBed } from '@angular/core/testing';

import { ErrorModal } from './error-modal';

describe('ErrorModal', () => {
  let service: ErrorModal;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ErrorModal);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
