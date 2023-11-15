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
  // {field: 'formName', header: 'AHU Number', isSortable: true, hasFilter: false,hasTableValue:true},
  //   {field: 'ClientName', header: 'Client Name', isSortable: true, hasFilter: true,hasTableValue:true},
  //   {field: 'DateCreated', header: 'Date Created', isSortable: true, hasFilter: true,hasTableValue:true}

 ]

 list:any[] =[
  {"id":1324,"formName":"ACH001","ClientName":"Sun Medicals pvt. Ltd","DateCreated":new Date().toDateString()},
  {"id":1326,"formName":"ACH002","ClientName":" MAcron Medicals pvt. Ltd","DateCreated":new Date().toDateString()},

  {"id":1325,"formName":"ACH003","ClientName":"Sathan Medicals pvt. Ltd","DateCreated":new Date().toDateString()},



]
}
