import { TestBed } from '@angular/core/testing';

import { CommonSpaceService } from './commonspace.service';

describe('CommonspaceService', () => {
  let service: CommonSpaceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CommonSpaceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
