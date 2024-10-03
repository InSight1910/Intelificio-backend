export interface Guest {
    id: number;
    firstname: string;
    lastname: string;
    rut: string;
    entrytime: Date;
    plate: string;
    unit: string;
    unitId: number;
    }

export type CreateGuest = Omit<Guest, 'id' | 'unit'>;

export interface UpdateGuest {
    id: number;
    firstname: string;
    lastname: string;
    rut: string;
    entrytime: Date;
    plate: string;
    unitId: number;
    }

export interface Building {
    id: number;
    name: string;
    units: number;
    }   

export interface Unit {
    id: number;
    floor: string;
    number: string;
    surface: number;
    user: string;
    building: string;
    unitType: string;
    buildingId?: number;
    unitTypeId?: number;
    }
