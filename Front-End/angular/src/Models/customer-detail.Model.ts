export class CustomerDetail {

    client: string ='';
    plant: string ='';
    equipmentId: string='';
    areaOfTest: string='';
    formType: FormType=FormType.ACPH;
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

