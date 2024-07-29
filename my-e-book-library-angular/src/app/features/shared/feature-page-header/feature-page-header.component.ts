import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';

@Component({
  selector: 'feature-page-header',
  standalone: true,
  imports: [MatFormField, MatLabel, MatIcon, FormsModule, MatInput, MatButtonModule, MatSuffix],
  templateUrl: './feature-page-header.component.html',
  styleUrl: './feature-page-header.component.scss'
})
export class FeaturePageHeaderComponent {
  @Input() headerText: string = '';
  @Input() public showSearch: boolean = true;
  @Input() public showAdd: boolean = true;
  @Output() searchEvent = new EventEmitter<string>();
  @Output() addButtonClickEvent = new EventEmitter();
  @Input() searchText: string = '';


  onSearchPressed($event: any) {
    if ($event instanceof KeyboardEvent) {
      if ($event.key === 'Enter') {
        this.applyFilter();
      }
    }
  }
  clearSearch() {
    this.searchText = '';
    this.applyFilter();
  }

  applyFilter() {
    this.searchEvent.emit(this.searchText);
  }

  onAddClick() {
    this.addButtonClickEvent.emit();
  }
}
