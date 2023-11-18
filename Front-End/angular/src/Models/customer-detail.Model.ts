export class CustomerDetail {
    id:number =0;
    client: string ='';
    plant: string ='';
    equipmentId: string='';
    areaOfTest: string='';
    formType: number=1;
    dateOfTest?: Date;
    instrumentId: number=0;
}

export enum FormType {
    ACPH = 1,
    FilterIntegrity,
    ParticleCountSingleCycle,
    ParticleCountThreeCycle,
    TempMapping
}

