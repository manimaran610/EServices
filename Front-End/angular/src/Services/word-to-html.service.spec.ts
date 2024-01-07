/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { WordToHtmlService } from './word-to-html.service';

describe('Service: WordToHtml', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WordToHtmlService]
    });
  });

  it('should ...', inject([WordToHtmlService], (service: WordToHtmlService) => {
    expect(service).toBeTruthy();
  }));
});
