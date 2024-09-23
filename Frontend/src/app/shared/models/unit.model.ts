export interface Unit {
  id?: number;
  floor: string;
  number: string;
  surface: number;
  user: string;
  building: number;
  unitType: number;
}

export interface UnitType {
  id: number;
  name: string;
}