/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { PosService } from './pos.service';

describe('Service: Pos', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PosService]
    });
  });

  it('should ...', inject([PosService], (service: PosService) => {
    expect(service).toBeTruthy();
  }));
});
