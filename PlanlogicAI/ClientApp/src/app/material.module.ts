import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatTooltipModule, MatTabsModule, MatSelectModule, MatInputModule, MatDatepickerModule, MatIconModule, MatAutocompleteModule } from '@angular/material';
@NgModule({
  imports: [MatButtonModule, MatTooltipModule, MatTabsModule, MatSelectModule, MatInputModule, MatDatepickerModule, MatIconModule, MatAutocompleteModule],
  exports: [MatButtonModule, MatTooltipModule, MatTabsModule, MatSelectModule, MatInputModule, MatDatepickerModule, MatIconModule, MatAutocompleteModule],
})
export class MaterialModule { }
