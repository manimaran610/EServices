import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { GridColumnOptions } from 'src/Models/grid-column-options';
import { GroupedColumnOptions } from 'src/Models/grouped-column-options';
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-form1',
  standalone:true,
  imports:[CommonModule,SharedModule],
  templateUrl: './form1.component.html',
  styleUrls: ['./form1.component.css']
})
export class Form1Component implements OnInit {

  options:GridColumnOptions[]=[
    {field: 'RoomName', header: 'Room Name / Number', isSortable: true, hasFilter: false,rowspan:'2'},
      {field: 'DesignACPH', header: 'Design ACPH', isSortable: true, hasFilter: true,rowspan:'2'},
      {field: 'GrillNo', header: 'Grill/Filter Reference No.', isSortable: true, hasFilter: true,rowspan:'2'},
      {field: 'FilterArea', header: 'FilterArea', isSortable: true, hasFilter: true,colspan:'1'},
      {field: 'AirVelocityInFPM', header: 'Air Velociy Reading in FPM', isSortable: true, hasFilter: true,colspan:'5'},
      {field: 'AvgVelocityInFPM', header: 'Avg Velocity FPM', isSortable: true, hasFilter: true,rowspan:'2'},
      {field: 'AirFlowCFM', header: 'Air Flow CFM', isSortable: true, hasFilter: true,rowspan:'2'},
      {field: 'TotalAirFlowCFM', header: 'Total Air Flow CFM', isSortable: true, hasFilter: true,rowspan:'2'},
      {field: 'RoomVolCFM', header: 'Room Volume CFT', isSortable: true, hasFilter: true,rowspan:'2'},
      {field: 'AirChanges', header: 'Air Changes per hour', isSortable: true, hasFilter: true,rowspan:'2'},
]

groupedOptions:GroupedColumnOptions[]=
[
  {gridColumnOptions:[
      {field: 'Product', header: 'Product', isSortable: true, hasFilter: true,rowspan:'3'},
      {field: 'sale', header: 'sale', isSortable: true, hasFilter: true,colspan:'4'}]
},
{gridColumnOptions:[
  {field: 'sales', header: 'sales', isSortable: true, hasFilter: true,colspan:'2'},
  {field: 'profit', header: 'profit', isSortable: true, hasFilter: true,colspan:'2'}]
},
{gridColumnOptions:[
  {field: 'Product1', header: 'Last Year'},
  {field: 'sale1', header: 'This Year'},
  {field: 'Product2', header: 'Last Year'},
  {field: 'sale2', header: 'This Year'}]
},

]
  
   list:any[] =[]
    // {"id":1324,"formName":"ACH001","ClientName":"Sun Medicals pvt. Ltd","DateCreated":new Date().toDateString()},
    // {"id":1326,"formName":"ACH002","ClientName":" MAcron Medicals pvt. Ltd","DateCreated":new Date().toDateString()},
  
    // {"id":1325,"formName":"ACH003","ClientName":"Sathan Medicals pvt. Ltd","DateCreated":new Date().toDateString()}];
  
  constructor() { }

  ngOnInit() {
  }

}
