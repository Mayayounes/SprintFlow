import { TestBed } from '@angular/core/testing';

import { NotificationSignalRService } from './notification-signal-rservice';

describe('NotificationSignalRService', () => {
  let service: NotificationSignalRService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NotificationSignalRService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
