import { OnChanges } from '@angular/core';
/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/explicit-member-accessibility */
import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SharedModule } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { GridColumnOptions } from 'src/Models/grid-column-options';
import { GroupedColumnOptions } from 'src/Models/grouped-column-options';


@Component({
    selector: 'app-grid',
    standalone: true,
    imports: [CommonModule, SharedModule, TableModule, FormsModule],
    schemas: [CUSTOM_ELEMENTS_SCHEMA],
    templateUrl: './grid.component.html',
    styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit, OnChanges {


    @ViewChild('singleSelectionTable', { static: false }) singleSelectionTable: any;

    @Input() title: string = '';
    @Input() listOfItems: any[] = [];
    @Input() groupedColumnOptions: GroupedColumnOptions[] = [];
    @Input() gridColumnOptions: GridColumnOptions[] = [];
    @Input() dataKey: any;
    @Input() rows: number = 20;
    @Input() rowsPerPageOptions: number[] = [20, 50, 100];
    @Input() hasradioButton: boolean = false;
    @Input() canEdit: boolean = false;
    @Input() canDelete: boolean = false;
    @Input() hasPagination: boolean = true;
    @Input() hasLazyLoading: boolean = false;
    @Input() totalRecords: number = 0;
    @Input() loadingSpinner: boolean = false;
    @Input() selected: any;
    @Input() hasRetension: boolean = false;
    @Input() hasColumnGroup: boolean = false;
    @Input() addNewRow: EventEmitter<any> = new EventEmitter<any>();


    @Output() Save: EventEmitter<any> = new EventEmitter<any>();
    @Output() Delete: EventEmitter<any> = new EventEmitter<any>();
    @Output() RadioChanges: EventEmitter<any> = new EventEmitter<any>();
    @Output() LazyLoad: EventEmitter<any> = new EventEmitter<any>();
    @Output() outList: any[]=this.listOfItems;

    firstOffset: number = 0;
    sortField: string | undefined;
    sortOrder: number = 1;


    selectedRow: any;
    editableRowData: any = {};
    clonedProducts: { [s: string]: any } = {};
    isNewRowInserted: boolean = false;

    getFilteredColumns = () => this.groupedColumnOptions.flatMap(group => group.gridColumnOptions).filter(option => option.hasTableValue && !option.isStandalone).sort((obj1, obj2) => obj1.orderNo! - obj2.orderNo!)

    getStandaloneColumns = () => this.groupedColumnOptions.flatMap(group => group.gridColumnOptions).filter(option => option.isStandalone);

    constructor() { }

    public ngOnInit() {
        this.selectedRow = this.selected;
    }

    ngOnChanges() {
        console.log('changes detected')
        this.addNewRow.subscribe((e) => {
            if (e == true) {
                this.addNewEditableRow()
            }
        })

    }

    onRadioSelected() { this.RadioChanges.emit(this.selectedRow); }

    onRowSelected(event: any) { }

    onLazyLoadEvent(event: any) {

        let queryParams = {
            offset: event.first,
            count: event.rows,
            sort: event.sortField !== undefined ? `${event.sortField}:${event.sortOrder === 1 ? 'asc' : 'desc'}` : '',

        };
        this.LazyLoad.emit(queryParams);
    }

    //Grid row crud operations
    addNewEditableRow() {
        this.editableRowData = {};
        this.editableRowData[this.dataKey] = this.generateRandomId();
        this.listOfItems = [this.editableRowData, ...this.listOfItems];
        this.singleSelectionTable.initRowEdit(this.editableRowData);
    }

    onRowEditInit(rowData: any, index: number) {
        this.singleSelectionTable.initRowEdit(rowData);
        this.clonedProducts[rowData[this.dataKey]] = { ...rowData };
    }

    onRowEditCancel(rowData: any, index: number) {
        if (this.clonedProducts[rowData[this.dataKey]] != null &&
            this.clonedProducts[rowData[this.dataKey]] !== undefined) {
            this.listOfItems[index] = this.clonedProducts[rowData[this.dataKey]];
        } else {
            this.listOfItems = [...this.listOfItems.filter((e) => e[this.dataKey] !== rowData[this.dataKey])];
        }
        this.singleSelectionTable.cancelRowEdit(rowData);
    }

    onRowEditSave(rowData: any, htmlTableRowElement: any) {
        this.Save.emit(rowData);
        // if (this.listOfItems.find(e => e[this.dataKey] === rowData[this.dataKey]) === undefined) {
        //     this.listOfItems=[this.listOfItems,rowData]
        //     console.log(this.listOfItems);
        // }
        // else {
        //     const index = this.listOfItems.findIndex(e => e[this.dataKey] === rowData[this.dataKey]);
        //     this.listOfItems[index] = rowData;
        //     console.log(this.listOfItems);

        // }
        this.singleSelectionTable.saveRowEdit(rowData, htmlTableRowElement);
        delete this.clonedProducts[rowData[this.dataKey]];
    }

    onRowDelete(rowData: any, index: number) {
        this.Delete.emit(rowData);
        this.listOfItems = [...this.listOfItems.filter((e) => e[this.dataKey] !== rowData[this.dataKey])];

    }

    private generateRandomId = (): string => {
        let result = '';
        const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        const charactersLength = characters.length;
        for (let i = 0; i < 12; i++) {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
        }
        return result;

    };

    calcList: any[] = [];
    calc(input: any): string {
        return this.calcList.length.toString();
    }
    calcIf(input: any): boolean {
        console.log('calcIf' + !this.calcList.includes(input))
        console.log(this.calcList)
        return !this.calcList.includes(input);
    }

}
