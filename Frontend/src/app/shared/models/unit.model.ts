export interface Unit {
  id?: number;
  floor: string;
  number: string;
  surface: number;
  user: string;
  building: number;
  unitType: number;
}

export interface CreateUnit {
  floor: string;
  number: string;
  surface: number;
  buildingId: number;
  unitTypeId: number;
}

export interface UnitType {
  id: number;
  name: string;
}