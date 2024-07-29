import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatFormField, MatInputModule, MatLabel, MatSuffix } from '@angular/material/input';
import { Observable } from 'rxjs/internal/Observable';
import { debounceTime, startWith, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-book-search-autocomplete',
  standalone: true,
  imports: [
    CommonModule,
    MatInputModule,
    MatFormField,
    MatLabel,
    MatIcon,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatSuffix,
    MatAutocompleteModule,
  ],
  templateUrl: './book-search-autocomplete.component.html',
  styleUrl: './book-search-autocomplete.component.scss'
})
export class BookSearchAutocompleteComponent implements OnInit {
  ngOnInit(): void {
    this.filteredItems = this.searchCtrl.valueChanges.pipe(
      startWith(''),
      debounceTime(300),
      switchMap(value => this.fetchListOfOptions(value || ''))
    );
  }
  labelText = '';
  textPropertyName = '';
  fetchListOfOptions!: (query: string | null) => Observable<any[]>;
  @Output() onItemSelected = new EventEmitter<any>();

  searchCtrl = new FormControl('');
  filteredItems!: Observable<any[]>;
  selctedItem: any = null;

  itemSelected(event: MatAutocompleteSelectedEvent) {
    this.selctedItem = event.option.value;
    this.onItemSelected.emit(this.selctedItem.id);
  }

  showText = (value: any): string => {
    if (value)
      return value[this.textPropertyName];
    return '';
  }

  clearItem() {
    this.selctedItem = null;
    this.searchCtrl.setValue(null);
    this.onItemSelected.emit(null);
  }
}
