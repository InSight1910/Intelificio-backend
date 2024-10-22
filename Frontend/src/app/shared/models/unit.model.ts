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
  userId?: number;
}

export interface CreateUnit {
  floor: string;
  number: string;
  surface: number;
  buildingId: number;
  unitTypeId: number;
  userId: number; 
}

export interface UpdateUnit {
  id: number;
  floor: string;
  number: string;
  surface: number;
  buildingId: number;
  unitTypeId: number;
  userId?: number;
}

export interface UnitType {
  id: number;
  name: string;
}
