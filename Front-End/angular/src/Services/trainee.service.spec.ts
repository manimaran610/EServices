/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { TraineeService } from './trainee.service';

describe('Service: Trainee', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TraineeService]
    });
  });

  it('should ...', inject([TraineeService], (service: TraineeService) => {
    expect(service).toBeTruthy();
  }));
});
