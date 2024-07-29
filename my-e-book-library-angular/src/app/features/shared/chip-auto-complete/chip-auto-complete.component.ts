import { Component, ElementRef, Input, OnInit, ViewChild, inject, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatChipsModule } from '@angular/material/chips';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { Observable, of } from 'rxjs';
import { debounceTime, map, startWith, switchMap } from 'rxjs/operators';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-chip-auto-complete',
  standalone: true,
  imports: [
    CommonModule,
    MatChipsModule,
    MatAutocompleteModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './chip-auto-complete.component.html',
  styleUrl: './chip-auto-complete.component.scss'
})
export class ChipAutoCompleteComponent implements OnInit {
  searchCtrl = new FormControl('');
  filteredOptions!: Observable<any[]>;
  @Input() selectedItems: any[] = [];
  @Input() options: any[] = [];
  @Input() textPropertyName: string = '';
  @Input() labelText: string = '';
  @Input() placeholderText: string = '';
  @Input() preloadedOptions: boolean = true;
  @Input() showAddButton: boolean = false;
  @Input() addButtonText: string = 'Add';

  @ViewChild('inputControl') inputControl!: ElementRef<HTMLInputElement>;

  @Input() fetchListOfOptions!: (query: string | null) => Observable<any[]>;

  onAddButtonClicked = output();

  constructor() {
  }

  ngOnInit(): void {
    if (this.preloadedOptions) {
      this.filteredOptions = this.searchCtrl.valueChanges.pipe(
        startWith(''),
        switchMap(value =>
          of(this.options.filter(o => o[this.textPropertyName].toLowerCase().includes(value)))
        )
      );
    }
    else {
      this.filteredOptions = this.searchCtrl.valueChanges.pipe(
        startWith(''),
        debounceTime(300),
        switchMap(value => this.fetchListOfOptions(value))
      );
    }
  }

  remove(item: any): void {
    const index = this.selectedItems.indexOf(item);
    if (index >= 0) {
      this.selectedItems.splice(index, 1);
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    const selectedOption = event.option.value;
    if (selectedOption) {
      const index = this.selectedItems.findIndex(d => d.id == selectedOption.id);
      if (index < 0) {
        this.selectedItems.push(selectedOption);
      }
    }
    this.searchCtrl.setValue(null);
  }

  addNewButtonClicked(event: any) {
    event.stopPropagation();
    this.onAddButtonClicked.emit();
  }
}
