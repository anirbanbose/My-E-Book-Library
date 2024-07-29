import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookCopyListComponent } from './book-copy-list.component';

describe('BookCopyListComponent', () => {
  let component: BookCopyListComponent;
  let fixture: ComponentFixture<BookCopyListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BookCopyListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookCopyListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
