import { LegalEntityType } from "./legal-entity-type.Model";

export class ApplicationServices{
    id?:string='';
    name?:string='';
    description?:string='';
    price?:number=0;
    legalEntityType?:LegalEntityType
    

}