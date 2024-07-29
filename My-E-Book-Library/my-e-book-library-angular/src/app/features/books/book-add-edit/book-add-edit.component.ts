import { AfterViewInit, Component, OnInit, ViewChild, inject } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { BookService } from '../../../_services/book/book.service';
import { AuthService } from '../../../_services/auth/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { FeaturePageHeaderComponent } from '../../shared/feature-page-header/feature-page-header.component';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipsModule } from '@angular/material/chips';
import { MatSelectModule } from '@angular/material/select';
import { ChipAutoCompleteComponent } from '../../shared/chip-auto-complete/chip-auto-complete.component';
import { LanguageService } from '../../../_services/language/language.service';
import { Observable, debounceTime, forkJoin, map, of, startWith, switchMap, tap } from 'rxjs';
import { AuthorService } from '../../../_services/author/author.service';
import { PublisherService } from '../../../_services/publisher/publisher.service';
import { GenreService } from '../../../_services/genre/genre.service';
import { CommonModule } from '@angular/common';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { provideNativeDateAdapter } from '@angular/material/core';
import { FileUploadComponent } from '../../shared/file-upload/file-upload.component';
import { HelperService } from '../../../_helpers/helper.service';
import { MatDividerModule } from '@angular/material/divider';
import { AuthorTypeService } from '../../../_services/author-type/author-type.service';
import { NgOptimizedImage } from '@angular/common'
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { ConfirmDialogData } from '../../shared/confirm-dialog/confirm-dialog-data';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';
import { BookFileUploadData } from '../book-file-upload/book-file-upload-data';
import { BookFileUploadComponent } from '../book-file-upload/book-file-upload.component';
import { AuthorAddEditComponent } from '../../authors/author-add-edit/author-add-edit.component';
import { PublisherAddEditComponent } from '../../publishers/publisher-add-edit/publisher-add-edit.component';
import { GenreAddEditComponent } from '../../genres/genre-add-edit/genre-add-edit.component';


@Component({
  selector: 'app-book-add-edit',
  standalone: true,
  imports: [
    CommonModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatCardModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
    MatDialogModule,
    FeaturePageHeaderComponent,
    MatChipsModule,
    MatAutocompleteModule,
    MatSelectModule,
    ChipAutoCompleteComponent,
    MatDatepickerModule,
    FileUploadComponent,
    MatDividerModule,
    NgOptimizedImage,
    NgxMaskDirective
  ],
  providers: [provideNativeDateAdapter(), provideNgxMask()],
  templateUrl: './book-add-edit.component.html',
  styleUrl: './book-add-edit.component.scss'
})
export class BookAddEditComponent implements AfterViewInit, OnInit {
  private fb = inject(FormBuilder);
  private _bookService = inject(BookService);
  private _authorService = inject(AuthorService);
  private _authorTypeService = inject(AuthorTypeService);
  private _publisherService = inject(PublisherService);
  private _genreService = inject(GenreService);
  private _languageService = inject(LanguageService);
  private _helperService = inject(HelperService);
  private _snackBar = inject(MatSnackBar);
  private _route = inject(ActivatedRoute);
  private _router = inject(Router);
  readonly dialog = inject(MatDialog);
  mode: string = 'Add';
  id: number = 0;
  headerText: string = 'Add New Book';
  authorTypes: any[] = [];
  filteredPublishers!: Observable<any[]>;
  filteredAuthors!: Observable<any[]>;
  imageSrc: string | ArrayBuffer | null = null;
  bookImageAdded: boolean = false;

  ebooks: any[] = [];
  allowedEbookFileTypes: string[] = ['application/pdf', 'application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', 'application/epub', 'application/mobi'];

  isFileUploaded: boolean = false;
  isFormSubmitted: boolean = false;

  allowedImageFileTypes: string[] = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif'];

  @ViewChild('languages') languages!: ChipAutoCompleteComponent;
  @ViewChild('genres') genres!: ChipAutoCompleteComponent;

  publisherSearchCtrl = new FormControl('');
  authorSearchCtrl = new FormControl('');

  errorMessages: string[] = [];
  bookForm = this.fb.group({
    id: [0],
    title: [null, [Validators.required, Validators.maxLength(200)]],
    subject: [null, Validators.maxLength(200)],
    iSBN10: [null, Validators.maxLength(10)],
    iSBN13: [null, Validators.maxLength(13)],
    description: [null, Validators.maxLength(500)],
    publisher: [null],
    editionName: [null, Validators.maxLength(300)],
    publishedDate: [null],
    noOfPages: [null, Validators.pattern("^[0-9]*$")],
    authors: this.fb.array([]),
  });



  constructor() {
    if (this._route.snapshot.paramMap.get('id')) {
      this.id = parseInt(this._route.snapshot.paramMap.get('id') as string);
      this.headerText = "Edit Book"
    }
  }
  ngOnInit(): void {
    this.getAuthorTypes();
    this.getBook();
    this.filteredPublishers = this.publisherSearchCtrl.valueChanges.pipe(
      startWith(''),
      //tap(() => this.bookForm.get('publisher')?.setValue(null)),
      //debounceTime(300),
      switchMap(value => this.fetchPublishers(value || ''))
    );
    this.filteredAuthors = this.authorSearchCtrl.valueChanges.pipe(
      startWith(''),
      debounceTime(300),
      switchMap(value => this.fetchAuthors(value || ''))
    );

  }
  ngAfterViewInit(): void {
  }

  getBook() {
    if (this.id > 0) {

      this._bookService.getBook(this.id).subscribe({
        next: (response: any) => {
          if (response && response.isSuccess) {
            var book = response.record;
            this.bookForm.patchValue({
              id: book.id,
              title: book.title,
              subject: book.subject,
              publisher: book.publisher,
              iSBN10: book.isbN10,
              iSBN13: book.isbN13,
              description: book.description,
              editionName: book.editionName,
              publishedDate: book.publishedDate,
              noOfPages: book.noOfPages
            });

            //publishedDate: book.publishedDate ? new Date(book.publishedDate) as any : null,



            this.publisherSearchCtrl.setValue(book.publisher);
            this.imageSrc = book.bookImage;
            this.genres.selectedItems = book.genres;
            this.languages.selectedItems = book.languages;
            book.authors.map((author: any) => {
              const authorControl = new FormGroup({
                'authorId': new FormControl(author.id),
                'authorName': new FormControl(author.authorName),
                'authorTypeId': new FormControl(author.authorTypeId)
              });
              (<FormArray>this.authors).push(authorControl);
            });
            this.ebooks = book.files.map((file: any) => {
              return {
                id: file.id,
                name: file.fileName,
                size: file.fileSize,
                type: file.fileType
              }
            })
            this.isFileUploaded = this.ebooks.length > 0;
          }
          else {
            this._snackBar.open("There was an error in retrieving the book record. Please try again later", 'Close', {
              duration: 5000
            });
            this._router.navigate(['/books']);
          }
        },
        error: (err: any) => {
          this._snackBar.open("There was an error in retrieving the book record. Please try again later", 'Close', {
            duration: 5000
          });
          this._router.navigate(['/books']);
        }
      })
    }
  }


  fetchLanguages = (query: string | null): Observable<any[]> => {
    return this._languageService.languageDropdownList(query);
  }

  fetchAuthors = (query: string | null): Observable<any[]> => {
    return this._authorService.authorDropdownList(query);
  }

  fetchGenres = (query: string | null): Observable<any[]> => {
    return this._genreService.genreDropdownList(query);
  }

  fetchPublishers = (query: string | null): Observable<any[]> => {
    return this._publisherService.publisherDropdownList(query);
  }

  publisherSelected($event: MatAutocompleteSelectedEvent) {
    this.bookForm.get('publisher')?.setValue($event.option.value);
  }
  authorSelected($event: MatAutocompleteSelectedEvent) {
    const authorControl = new FormGroup({
      'authorId': new FormControl($event.option.value.id),
      'authorName': new FormControl($event.option.value.fullName),
      'authorTypeId': new FormControl(8) //Other is selected by default
    });
    (<FormArray>this.authors).push(authorControl);
    this.authorSearchCtrl?.setValue(null);
  }

  showPublisherText = (value: any): string => {
    if (value)
      return value.publisherName;
    return '';
  }

  showAuthorText = (value: any): string => {
    if (value)
      return value.fullName;
    return '';
  }

  onSubmit() {
    this.errorMessages = [];
    this.isFileUploaded = false;
    this.isFormSubmitted = true;

    if (this.ebooks.length > 0) {
      this.isFileUploaded = true;
    }
    if (this.bookForm.invalid || !this.isFileUploaded) {
      return;
    }
    const formData = this.bookForm.value;

    const selectedPublisher = formData.publisher ? formData.publisher as any : null;
    let payload: any = {
      id: formData.id,
      title: formData.title,
      bookImage: this.imageSrc,
      imageUploaded: this.bookImageAdded,
      subject: formData.subject,
      iSBN10: formData.iSBN10,
      iSBN13: formData.iSBN13,
      description: formData.description,
      editionName: formData.editionName,
      publishedDate: formData.publishedDate ? this._helperService.dateWithoutTimezone(new Date(formData.publishedDate)) : null,
      noOfPages: formData.noOfPages,
      publisher: selectedPublisher ? { id: selectedPublisher.id, publisherName: selectedPublisher.publisherName } : null,
      languages: this.languages.selectedItems,
      genres: this.genres.selectedItems,
      authors: this.authors.controls.map((d: any) => {
        if (d && d.value) {
          return { id: d.value.authorId, authorName: d.value.authorName, authorTypeId: d.value.authorTypeId }
        }
        return { id: -100, authorName: '', authorTypeId: -100 }
      })
    };
    payload.files = [];

    let existingFiles = this.ebooks.filter(d => d.id > 0).map((file: any) => {
      return { id: file.id, fileName: file.name, fileType: file.type, fileSize: file.size, fileData: null }
    });
    if (existingFiles && existingFiles.length > 0) {
      payload.files.push(...existingFiles);
    }

    let addedFiles = this.ebooks.filter(d => !d.id || d.id == 0);
    if (addedFiles.length > 0) {
      const fileReadObservables = addedFiles.map(file => this._helperService.readFile(file));

      forkJoin(fileReadObservables).subscribe(fileContents => {
        fileContents.forEach((content, index) => {
          payload.files.push(content);
        });
        this.saveBook(payload);
      });
    }
    else {
      this.saveBook(payload);
    }
  }

  saveBook(bookPaylod: any) {
    if (bookPaylod) {
      this._bookService.saveBook(bookPaylod).subscribe({
        next: (response: any) => {
          if (response.isSuccess) {
            this._snackBar.open("Book saved successfully.", 'Close', {
              duration: 5000
            });
            this._router.navigate(['/books']);
          }
          else {
            this._snackBar.open("Book couldn't be saved. Please try again later.", 'Close', {
              duration: 5000
            });
          }
        },
        error: (err) => {
          if (err.status == 422) {
            this.errorMessages = this._helperService.getErrorMessages(err);
          }
          else {
            this._snackBar.open("There was an error saving the book. Please try again later", 'Close', {
              duration: 5000
            });
          }

        }
      });
    }
  }

  get authors(): FormArray {
    return this.bookForm.get('authors') as FormArray;
  }

  getAuthorTypes() {
    this._authorTypeService.authorTypeDropdownList().subscribe({
      next: (response: any) => {
        if (response && response.isSuccess)
          this.authorTypes = response.records;
      }
    })
  }


  deleteAuthor(index: number) {
    this.authors.controls.splice(index, 1);
  }

  deleteFile(index: number) {
    this.ebooks.splice(index, 1);
    this.isFileUploaded = this.ebooks.length > 0;
  }

  deleteImage() {
    this.imageSrc = null;
  }

  formatBytes(bytes: number) {
    if (bytes === 0) {
      return '0 Bytes';
    }
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }

  backToList() {
    if (this.bookForm.dirty) {
      let data: ConfirmDialogData = {
        title: 'Lose the changes?',
        message: 'There are unsaved changes in the page. Are you sure you don\'t want to save them before moving away?',
        OkButtonText: "Yes",
        cancelButtonText: "No"
      }
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        height: '200px',
        width: '400px',
        data: data
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result !== undefined) {
          this._router.navigate(['/books'])
        }
      });
    }

  }

  openBookCoverDialog() {
    let data: BookFileUploadData = {
      title: 'Upload Book Cover Image',
      maxFileSize: 2,
      allowMultiple: false,
      allowedFileTypes: this.allowedImageFileTypes,
      uploaderText: 'Drag & Drop Book Cover Image Here',
      fileHint: 'Only JPEG, PNG, and GIF image files having the size of 2MB or less are allowed.'
    }
    const dialogRef = this.dialog.open(BookFileUploadComponent, {
      height: '450px',
      width: '500px',
      data: data
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        const file = result[0];
        const reader = new FileReader();
        reader.onload = e => this.imageSrc = reader.result;

        reader.readAsDataURL(file);
        this.bookImageAdded = true;
      }
    });
  }

  openBookCopyDialog() {
    let data: BookFileUploadData = {
      title: 'Upload Ebook files',
      maxFileSize: 50,
      allowMultiple: true,
      allowedFileTypes: this.allowedEbookFileTypes,
      uploaderText: 'Drag & Drop Ebook Files Here',
      fileHint: 'Only PDF, DOC/DOCX, EPUB and MOBI files having the size of 50MB or less are allowed.'
    }
    const dialogRef = this.dialog.open(BookFileUploadComponent, {
      height: '450px',
      width: '500px',
      data: data
    });

    dialogRef.afterClosed().subscribe(files => {
      if (files && files.length > 0) {
        this.ebooks.push(...files);
        this.isFileUploaded = this.ebooks.length > 0;
      }
    });
  }

  addNewPublisherClicked(event: Event) {
    event.stopPropagation();
    const dialogRef = this.dialog.open(PublisherAddEditComponent, {
      height: '250px',
      width: '400px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.publisherSearchCtrl.setValue(result);
        this.bookForm.get('publisher')?.setValue(result);
      }
    });
  }
  addNewAuthorClicked(event: Event) {
    event.stopPropagation();
    const dialogRef = this.dialog.open(AuthorAddEditComponent, {
      height: '400px',
      width: '400px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        const authorControl = new FormGroup({
          'authorId': new FormControl(result.id),
          'authorName': new FormControl(`${result.firstName} ${result.middleName ? (result.middleName + ' ') : ''} ${result.lastName}`),
          'authorTypeId': new FormControl(8)
        });
        (<FormArray>this.authors).push(authorControl);
      }
    });
  }

  addNewGenreClicked() {
    const dialogRef = this.dialog.open(GenreAddEditComponent, {
      height: '250px',
      width: '400px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.genres.selectedItems.push(result);
      }
    });
  }

  clearPublisher() {
    this.publisherSearchCtrl.setValue(null);
    this.bookForm.get('publisher')?.setValue(null);
  }
}
