import { RoomLocation } from "./room-Location.Model";
import { RoomGrill } from "./room-grill.Model";

export class Room {
    id: number = 0
    name: string = '';
    designACPH: string = '';
    areaM2: number = 0;
    limit: string = '';
    classType: string = '';
    noOfGrills: number = 0;
    noOfLocations: number = 0;
    roomVolume: number = 0;
    totalAirFlowCFM: number = 0;
    airChangesPerHour: number = 0;
    customerDetailId: number = 0;
    roomGrills: RoomGrill[] = [];
    roomLocations: RoomLocation[] = [];

}
