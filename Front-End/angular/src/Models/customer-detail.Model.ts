export class CustomerDetail {
  id: number = 0;
  client: string = '';
  plant: string = '';
  equipmentId: string = '';
  areaOfTest: string = '';
  formType: number = 1;
  formTypeName: string = '';
  classType: string = '';
  testCondition: string = '';
  traineeId: number = 0;
  dateOfTest?: Date;
  dateOfTestDue?: Date;

  instrumentId: number = 0;
}

export enum FormType {
  ACPH = 1,
  FilterIntegrity,
  ParticleCountSingleCycle,
  ParticleCountThreeCycle,
  TempMapping
}
