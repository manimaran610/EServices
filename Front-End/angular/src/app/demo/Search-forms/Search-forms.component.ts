import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { GridColumnOptions } from 'src/Models/grid-column-options';
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-Search-forms',
  standalone: true,
  imports: [CommonModule, SharedModule],
  templateUrl: './Search-forms.component.html',
  styleUrls: ['./Search-forms.component.css']
})
export class SearchFormsComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    console.log("search-forms-componenet")
  }
 options:GridColumnOptions[]=[
  {field: 'name', header: 'Name', isSortable: true, hasFilter: false,isEditable:true},
    {field: 'Age', header: 'Age', isSortable: true, hasFilter: true},
    {field: 'exp', header: 'Experience', isSortable: true, hasFilter: true},

 ]

 list:any[] =[
  {"id":1324,"name":"mani","Age":24,"exp":2},
  {"id":1325,"name":"maran","Age":10,"exp":7},

  {"id":1326,"name":"arun","Age":99,"exp":1},



]
}
