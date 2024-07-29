import { TestBed } from '@angular/core/testing';

import { AuthorTypeService } from './author-type.service';

describe('AuthorTypeService', () => {
  let service: AuthorTypeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthorTypeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
