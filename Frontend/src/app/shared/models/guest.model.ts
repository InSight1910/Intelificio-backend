export interface Guest {
    id: number;
    firstName: string;
    lastName: string;
    rut: string;
    entryTime: string;
    plate: string;
    unit: string;
    }

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
    firstname: string;
    lastname: string;
    rut: string;
    entryTime: Date;
    plate: string;
    unit: number;
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
