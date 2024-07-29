import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookListHeaderComponent } from './book-list-header.component';

describe('BookListHeaderComponent', () => {
  let component: BookListHeaderComponent;
  let fixture: ComponentFixture<BookListHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BookListHeaderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookListHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
