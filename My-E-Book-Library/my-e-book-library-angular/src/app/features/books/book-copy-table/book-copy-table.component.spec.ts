import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookCopyTableComponent } from './book-copy-table.component';

describe('BookCopyTableComponent', () => {
  let component: BookCopyTableComponent;
  let fixture: ComponentFixture<BookCopyTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BookCopyTableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookCopyTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
