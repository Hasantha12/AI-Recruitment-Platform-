import { TestBed } from '@angular/core/testing';

import { CandidateProfile } from './candidate-profile';

describe('CandidateProfile', () => {
  let service: CandidateProfile;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CandidateProfile);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
