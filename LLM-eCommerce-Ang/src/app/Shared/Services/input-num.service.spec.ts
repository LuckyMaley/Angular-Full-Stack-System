import { TestBed } from '@angular/core/testing';

import { InputNumService } from './input-num.service';

describe('InputNumService', () => {
  let service: InputNumService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InputNumService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
