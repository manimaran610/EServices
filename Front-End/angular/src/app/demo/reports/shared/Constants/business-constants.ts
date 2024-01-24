
/* eslint-disable @typescript-eslint/no-explicit-any */
export class BusinessConstants {

    public static ClassificationTypes: any[] = [
        {
            name: 'Class 5',
            pointFiveMicron: 3520,
            oneMicron:832,
            fiveMicron: 20
        },
        {
            name: 'Class 6',
            pointFiveMicron: 35200,
            oneMicron:8320,
            fiveMicron: 293
        },
        {
            name: 'Class 7',
            pointFiveMicron: 352000,
            oneMicron:83200,
            fiveMicron: 2930
        },
        {
            name: 'Class 8',
            pointFiveMicron: 3520000,
            oneMicron:832000,
            fiveMicron: 29300
        }
    ]

    public static ISO14644_Clean_Room_Samples: any[] = [
        { cleanroomArea: 2, minimumSample: 1 },
        { cleanroomArea: 4, minimumSample: 2 },
        { cleanroomArea: 6, minimumSample: 3 },
        { cleanroomArea: 8, minimumSample: 4 },
        { cleanroomArea: 10, minimumSample: 5 },
        { cleanroomArea: 24, minimumSample: 6 },
        { cleanroomArea: 28, minimumSample: 7 },
        { cleanroomArea: 32, minimumSample: 8 },
        { cleanroomArea: 36, minimumSample: 9 },
        { cleanroomArea: 52, minimumSample: 10 },
        { cleanroomArea: 56, minimumSample: 11 },
        { cleanroomArea: 64, minimumSample: 12 },
        { cleanroomArea: 68, minimumSample: 13 },
        { cleanroomArea: 72, minimumSample: 14 },
        { cleanroomArea: 76, minimumSample: 15 },
        { cleanroomArea: 104, minimumSample: 16 },
        { cleanroomArea: 108, minimumSample: 17 },
        { cleanroomArea: 116, minimumSample: 18 },
        { cleanroomArea: 148, minimumSample: 19 },
        { cleanroomArea: 156, minimumSample: 20 },
        { cleanroomArea: 192, minimumSample: 21 },
        { cleanroomArea: 232, minimumSample: 22 },
        { cleanroomArea: 276, minimumSample: 23 },
        { cleanroomArea: 352, minimumSample: 24 },
        { cleanroomArea: 436, minimumSample: 25 },
        { cleanroomArea: 636, minimumSample: 26 },
        { cleanroomArea: 1000, minimumSample: 27 }
    ]

    public static InstrumentTypes: string[] = [
        'Anemometer',
        'Capture Hoot',
        'Particle Counter',
        'Aerosol Photometer'   
    ]

}


