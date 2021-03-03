/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { HumanResourceService } from './humanResource.service';

describe('Service: HumanResource', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HumanResourceService]
    });
  });

  it('should ...', inject([HumanResourceService], (service: HumanResourceService) => {
    expect(service).toBeTruthy();
  }));
});
