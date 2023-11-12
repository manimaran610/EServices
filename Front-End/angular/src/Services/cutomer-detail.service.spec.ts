/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { CutomerDetailService } from './cutomer-detail.service';

describe('Service: CutomerDetail', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CutomerDetailService]
    });
  });

  it('should ...', inject([CutomerDetailService], (service: CutomerDetailService) => {
    expect(service).toBeTruthy();
  }));
});
