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

//   options:GridColumnOptions[]=[
//     {field: 'RoomName', header: 'Room Name / Number', isSortable: true, hasFilter: false,rowspan:'2' },
//       {field: 'DesignACPH', header: 'Design ACPH', isSortable: true, hasFilter: true,rowspan:'2'},
//       {field: 'GrillNo', header: 'Grill/Filter Reference No.', isSortable: true, hasFilter: true,rowspan:'2'},
//       {field: 'FilterArea', header: 'FilterArea', isSortable: true, hasFilter: true,colspan:'1'},
//       {field: 'AirVelocityInFPM', header: 'Air Velociy Reading in FPM', isSortable: true, hasFilter: true,colspan:'5'},
//       {field: 'AvgVelocityInFPM', header: 'Avg Velocity FPM', isSortable: true, hasFilter: true,rowspan:'2'},
//       {field: 'AirFlowCFM', header: 'Air Flow CFM', isSortable: true, hasFilter: true,rowspan:'2'},
//       {field: 'TotalAirFlowCFM', header: 'Total Air Flow CFM', isSortable: true, hasFilter: true,rowspan:'2'},
//       {field: 'RoomVolCFM', header: 'Room Volume CFT', isSortable: true, hasFilter: true,rowspan:'2'},
//       {field: 'AirChanges', header: 'Air Changes per hour', isSortable: true, hasFilter: true,rowspan:'2'},
// ]

groupedOptions:GroupedColumnOptions[]=
[
  {gridColumnOptions:[
      {field: 'product', header: 'Product', isSortable: true, hasFilter: true,rowspan:'3',colspan:undefined,hasTableValue:true},
      {field: 'sale', header: 'sale', rowspan:undefined,colspan:'4',hasTableValue:false}]
},
{gridColumnOptions:[
  {field: 'sales', header: 'sales', rowspan:undefined,colspan:'2',hasTableValue:false},
  {field: 'profit', header: 'profit',rowspan:undefined, colspan:'2',hasTableValue:false}]
},
{gridColumnOptions:[
  {field: 'lastYearSale', header: 'Last Year',hasTableValue:true,isSortable: true, hasFilter: true,},
  {field: 'thisYearSale', header: 'This Year',hasTableValue:true,isSortable: true, hasFilter: true},
  {field: 'lastYearProfit', header: 'Last Year',hasTableValue:true,isSortable: true, hasFilter: true},
  {field: 'thisYearProfit', header: 'This Year',hasTableValue:true,isSortable: true, hasFilter: true}]
},
]

tableColumns:GridColumnOptions[]=[];

list:any[] =[
  //  {id:1,product:'product',product1:'1',sale1:'2',product2:'3',sale2:'4'},
  //  {id:2,product:'product',product1:'1',sale1:'2',product2:'3',sale2:'4'},
  //  {id:3,product:'product',product1:'1',sale1:'2',product2:'3',sale2:'4'},
  //  {product:'product',product1:'1',sale1:'2',product2:'3',sale2:'4'},
  //  {product:'product',product1:'1',sale1:'2',product2:'3',sale2:'4'},

  // ]
            { product: 'Black Watch', lastYearSale: 83, thisYearSale: 9, lastYearProfit: 423132, thisYearProfit: 312122 },
            { product: 'Blue Band', lastYearSale: 38, thisYearSale: 5, lastYearProfit: 12321, thisYearProfit: 8500 },
            { product: 'Blue T-Shirt', lastYearSale: 49, thisYearSale: 22, lastYearProfit: 745232, thisYearProfit: 65323 },
            { product: 'Brown Purse', lastYearSale: 17, thisYearSale: 79, lastYearProfit: 643242, thisYearProfit: 500332 },
            { product: 'Chakra Bracelet', lastYearSale: 52, thisYearSale: 65, lastYearProfit: 421132, thisYearProfit: 150005 },
            { product: 'Galaxy Earrings', lastYearSale: 82, thisYearSale: 12, lastYearProfit: 131211, thisYearProfit: 100214 },
            { product: 'Game Controller', lastYearSale: 44, thisYearSale: 45, lastYearProfit: 66442, thisYearProfit: 53322 },
            { product: 'Gaming Set', lastYearSale: 90, thisYearSale: 56, lastYearProfit: 765442, thisYearProfit: 296232 },
            { product: 'Gold Phone Case', lastYearSale: 75, thisYearSale: 54, lastYearProfit: 21212, thisYearProfit: 12533 }
        ];
    // {"id":1324,"formName":"ACH001","ClientName":"Sun Medicals pvt. Ltd","DateCreated":new Date().toDateString()},
    // {"id":1326,"formName":"ACH002","ClientName":" MAcron Medicals pvt. Ltd","DateCreated":new Date().toDateString()},
  
    // {"id":1325,"formName":"ACH003","ClientName":"Sathan Medicals pvt. Ltd","DateCreated":new Date().toDateString()}];
  
  constructor() { }

  ngOnInit() {
  }

}
