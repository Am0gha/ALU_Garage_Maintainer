import { TestBed } from '@angular/core/testing';

import { AluApi } from './alu-api';

describe('AluApi', () => {
  let service: AluApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AluApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
