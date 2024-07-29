import { Component, ComponentRef, ElementRef, EventEmitter, inject, Input, model, OnInit, Output, QueryList, Type, ViewChild, ViewChildren, ViewContainerRef } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInput, MatInputModule } from '@angular/material/input';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { Observable } from 'rxjs/internal/Observable';
import { AuthorService } from '../../../_services/author/author.service';
import { PublisherService } from '../../../_services/publisher/publisher.service';
import { GenreService } from '../../../_services/genre/genre.service';
import { LanguageService } from '../../../_services/language/language.service';
import { CommonModule } from '@angular/common';
import { BookSearchAutocompleteComponent } from '../book-search-autocomplete/book-search-autocomplete.component';
import { BookSearchFilter } from '../../../models/BookSearchFilter';
import { MatMenuModule } from '@angular/material/menu';
enum ControlType {
  Author = 1,
  Publisher,
  Genre,
  Language,
}

interface LoadedComponent {
  componentRef: ComponentRef<BookSearchAutocompleteComponent>;
  controlType: ControlType
}

@Component({
  selector: 'app-book-list-header',
  standalone: true,
  imports: [
    CommonModule,
    MatInputModule,
    MatFormField,
    MatLabel,
    MatIcon,
    FormsModule,
    ReactiveFormsModule,
    MatInput,
    MatButtonModule,
    MatSuffix,
    MatButtonToggleModule,
    MatCheckboxModule,
    MatAutocompleteModule,
    BookSearchAutocompleteComponent,
    MatMenuModule
  ],
  templateUrl: './book-list-header.component.html',
  styleUrl: './book-list-header.component.scss'
})
export class BookListHeaderComponent implements OnInit {

  ngOnInit(): void {
    if (this.searchFilter) {
      if (this.searchFilter.searchText.trim() != '') {
        this.searchType = 'B';
        this.searchFilter.authorId = null;
        this.searchFilter.publisherId = null;
        this.searchFilter.genreId = null;
        this.searchFilter.languageId = null;
      }
      else if (this.searchFilter.authorId != null || this.searchFilter.publisherId != null || this.searchFilter.genreId != null ||
        this.searchFilter.languageId != null) {
        this.searchType = 'A';
        if (this.searchFilter.authorId != null) {
          this.authorSearch.set(true);
          this.getAuthor(this.searchFilter.authorId);
        }
        if (this.searchFilter.publisherId != null) {
          this.publisherSearch.set(true);
          this.getPublisher(this.searchFilter.publisherId);
        }
        if (this.searchFilter.genreId != null) {
          this.genreSearch.set(true);
          this.getGenre(this.searchFilter.genreId);
        }
        if (this.searchFilter.languageId != null) {
          this.languageSearch.set(true);
          this.getLanguage(this.searchFilter.languageId);
        }
      }
    }
  }

  @Input() headerText: string = '';
  @Output() searchEvent = new EventEmitter<BookSearchFilter>();
  @Output() addButtonClickEvent = new EventEmitter();
  searchText: string = '';
  private _authorService = inject(AuthorService);
  private _publisherService = inject(PublisherService);
  private _genreService = inject(GenreService);
  private _languageService = inject(LanguageService);

  @Input() searchFilter: BookSearchFilter = {
    authorId: null,
    genreId: null,
    languageId: null,
    publisherId: null,
    searchText: '',
    sortColumn: 'Title',
    isSortAscending: true
  };

  @ViewChild('dynamicComponentContainer', { read: ViewContainerRef, static: true }) container!: ViewContainerRef;
  componentRefs: LoadedComponent[] = [];


  searchType: string = 'B';
  authorSearch = model(false);
  publisherSearch = model(false);
  genreSearch = model(false);
  languageSearch = model(false);
  filteredPublishers!: Observable<any[]>;
  filteredAuthors!: Observable<any[]>;
  filteredGenres!: Observable<any[]>;
  filteredLanguages!: Observable<any[]>;

  onSearchPressed($event: any) {
    if ($event instanceof KeyboardEvent) {
      if ($event.key === 'Enter') {
        this.searchFilter.searchText = this.searchText;
        this.applyFilter();
      }
    }
  }

  clearSearch() {
    this.searchFilter = {
      authorId: null,
      genreId: null,
      languageId: null,
      publisherId: null,
      searchText: '',
      sortColumn: 'Title',
      isSortAscending: true
    }
    this.searchText = '';
    this.applyFilter();
  }

  applyFilter() {
    this.searchEvent.emit(this.searchFilter);
  }

  onAddClick() {
    this.addButtonClickEvent.emit();
  }

  onSearchTypeChange($event: any) {
    this.clearSearch();
    this.unloadAllComponents();
    this.unchcekAllCheckboxes();
  }

  onAuthorSearchChecked() {
    if (this.authorSearch()) {
      this.loadComponent(ControlType.Author);
    }
    else {
      this.unloadComponent(ControlType.Author);
    }

  }
  onPublisherSearchChecked() {
    if (this.publisherSearch()) {
      this.loadComponent(ControlType.Publisher);
    }
    else {
      this.unloadComponent(ControlType.Publisher);
    }
  }
  onLanguageSearchChecked() {
    if (this.languageSearch()) {
      this.loadComponent(ControlType.Language);
    }
    else {
      this.unloadComponent(ControlType.Language);
    }
  }
  onGenreSearchChecked() {
    if (this.genreSearch()) {
      this.loadComponent(ControlType.Genre);
    }
    else {
      this.unloadComponent(ControlType.Genre);
    }
  }

  fetchPublishers = (query: string | null): Observable<any[]> => {
    return this._publisherService.publisherDropdownList(query);
  }

  publisherSelected($event: any) {
    this.searchFilter.publisherId = $event;
    this.applyFilter();
  }

  fetchGenres = (query: string | null): Observable<any[]> => {
    return this._genreService.genreDropdownList(query);
  }

  genreSelected($event: any) {
    this.searchFilter.genreId = $event;
    this.applyFilter();
  }

  fetchAuthors = (query: string | null): Observable<any[]> => {
    return this._authorService.authorDropdownList(query);
  }

  authorSelected($event: any) {
    this.searchFilter.authorId = $event;
    this.applyFilter();
  }

  fetchLanguages = (query: string | null): Observable<any[]> => {
    return this._languageService.languageDropdownList(query);
  }

  languageSelected($event: any) {
    this.searchFilter.languageId = $event;
    this.applyFilter();
  }

  loadComponent(controlType: ControlType, value: any = null) {
    const componentRef: ComponentRef<any> = this.container.createComponent(BookSearchAutocompleteComponent);
    switch (controlType) {
      case ControlType.Author:
        componentRef.instance.labelText = 'Author';
        componentRef.instance.textPropertyName = 'fullName';
        componentRef.instance.fetchListOfOptions = this.fetchAuthors;
        componentRef.instance.onItemSelected.subscribe((event: any) => this.authorSelected(event));
        break;
      case ControlType.Publisher:
        componentRef.instance.labelText = 'Publisher';
        componentRef.instance.textPropertyName = 'publisherName';
        componentRef.instance.fetchListOfOptions = this.fetchPublishers;
        componentRef.instance.onItemSelected.subscribe((event: any) => this.publisherSelected(event));
        break;
      case ControlType.Genre:
        componentRef.instance.labelText = 'Genre';
        componentRef.instance.textPropertyName = 'genreName';
        componentRef.instance.fetchListOfOptions = this.fetchGenres;
        componentRef.instance.onItemSelected.subscribe((event: any) => this.genreSelected(event));
        break;
      case ControlType.Language:
        componentRef.instance.labelText = 'Language';
        componentRef.instance.textPropertyName = 'languageName';
        componentRef.instance.fetchListOfOptions = this.fetchLanguages;
        componentRef.instance.onItemSelected.subscribe((event: any) => this.languageSelected(event));
        break;
    }
    if (value) {
      componentRef.instance.searchCtrl.setValue(value);
    }
    this.componentRefs.push({ componentRef: componentRef, controlType: controlType });
  }

  unloadComponent(controlType: ControlType) {
    let index = this.componentRefs.map(function (o) {
      return o.controlType;
    }).indexOf(controlType);

    if (this.componentRefs[index]) {
      this.componentRefs[index].componentRef.destroy();
      this.componentRefs.splice(index, 1);
    }
  }

  unloadAllComponents() {
    for (let i = 0; i < this.componentRefs.length; i++) {
      this.componentRefs[i].componentRef.destroy();
    }
    this.componentRefs = [];
  }

  unchcekAllCheckboxes() {
    this.authorSearch.set(false);
    this.publisherSearch.set(false);
    this.genreSearch.set(false);
    this.languageSearch.set(false);
  }


  getPublisher(publisherId: number) {
    this._publisherService.getPublisher(publisherId).subscribe({
      next: (result: any) => {
        if (result.isSuccess) {
          this.loadComponent(ControlType.Publisher, result.record);
        }
      },
      error: (err: any) => {
        //do nothing
      }
    })
  }
  getAuthor(authorId: number) {
    this._authorService.getAuthor(authorId).subscribe({
      next: (result: any) => {
        if (result.isSuccess) {
          this.loadComponent(ControlType.Author, result.record);
        }
      },
      error: (err: any) => {
        //do nothing
      }
    })
  }
  getGenre(genreId: number) {
    this._genreService.getGenre(genreId).subscribe({
      next: (result: any) => {
        if (result.isSuccess) {
          this.loadComponent(ControlType.Genre, result.record);
        }
      },
      error: (err: any) => {
        //do nothing
      }
    })
  }
  getLanguage(languageId: number) {
    this._languageService.getLanguage(languageId).subscribe({
      next: (result: any) => {
        if (result.isSuccess) {
          this.loadComponent(ControlType.Language, result.record);
        }
      },
      error: (err: any) => {
        //do nothing
      }
    })
  }

  sortClicked(columnName: string) {
    if (this.searchFilter.sortColumn == columnName) {
      this.searchFilter.isSortAscending = !this.searchFilter.isSortAscending;
    }
    else {
      this.searchFilter.sortColumn = columnName;
      this.searchFilter.isSortAscending = true;
    }
    this.applyFilter();
  }
}


