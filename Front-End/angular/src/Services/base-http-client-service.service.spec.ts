/* tslint:disable:no-unused-variable */

import { TestBed, inject } from '@angular/core/testing';
import { BaseHttpClientServiceService } from './base-http-client-service.service';

describe('Service: BaseHttpClientService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BaseHttpClientServiceService]
    });
  });

  it('should ...', inject([BaseHttpClientServiceService], (service: BaseHttpClientServiceService) => {
    expect(service).toBeTruthy();
  }));
});
