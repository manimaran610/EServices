/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { FileProcessingService } from './file-processing.service';

describe('Service: FileProcessing', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FileProcessingService]
    });
  });

  it('should ...', inject([FileProcessingService], (service: FileProcessingService) => {
    expect(service).toBeTruthy();
  }));
});
