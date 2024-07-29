import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookSearchAutocompleteComponent } from './book-search-autocomplete.component';

describe('BookSearchAutocompleteComponent', () => {
  let component: BookSearchAutocompleteComponent;
  let fixture: ComponentFixture<BookSearchAutocompleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BookSearchAutocompleteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookSearchAutocompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
