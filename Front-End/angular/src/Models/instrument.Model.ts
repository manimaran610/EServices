export class Instrument {
    id: number = 0
    serialNo: string = '';
    make: string = '';
    model: string = '';
    type:string ='';
    calibratedOn?: Date;
    calibratedDueOn?: Date;
    certificateName?: string;
    certificateFile?: any;

}
