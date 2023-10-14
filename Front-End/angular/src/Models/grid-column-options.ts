export class GridColumnOptions {
    public field: string = '';
    public header: string = '';
    public isSortable?: boolean = false;
    public isEditable?: boolean = false;
    public hasFilter?: boolean = false;
    public width?: string = '';
    public hasRouterLink?: boolean = false;
    public routerLink?: string;
    public hasDropDownOptions?: boolean = false;
    public dropDownOptions?: string[] = [];


}
