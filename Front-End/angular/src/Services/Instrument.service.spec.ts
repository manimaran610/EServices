/* tslint:disable:no-unused-variable */

import { TestBed, inject } from '@angular/core/testing';
import { InstrumentService } from './Instrument.service';

describe('Service: InstrumentService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [InstrumentService]
    });
  });

  it('should ...', inject([InstrumentService], (service: InstrumentService) => {
    expect(service).toBeTruthy();
  }));
});
