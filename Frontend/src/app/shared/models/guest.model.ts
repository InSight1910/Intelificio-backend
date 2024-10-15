export interface Guest {
    id: number;
    firstName: string;
    lastName: string;
    rut: string;
    entryTime: string;
    plate: string;
    buildingId: number;
    unitId: number;
    }

export type GuestList = Omit<Guest, 'buildingId' | 'unitId'> & { unit: string;};

export interface CreateGuest {
    firstName: string;
    lastName: string;
    rut: string;
    entryTime: string;
    plate: string;
    unitId: number;
    }

export interface UpdateGuest {
    id: number;
    firstName: string;
    lastName: string;
    rut: string;
    entryTime: Date;
    plate: string;
    unitId: number;
    } 

