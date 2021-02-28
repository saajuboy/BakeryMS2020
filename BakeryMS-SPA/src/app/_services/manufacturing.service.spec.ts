/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ManufacturingService } from './manufacturing.service';

describe('Service: Manufacturing', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ManufacturingService]
    });
  });

  it('should ...', inject([ManufacturingService], (service: ManufacturingService) => {
    expect(service).toBeTruthy();
  }));
});
