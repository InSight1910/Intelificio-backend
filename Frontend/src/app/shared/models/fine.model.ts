import {Community} from "./community.model";

export interface Fine {
  fineId: number;
  name: string;
  amount: number;
  status: FineDenomination;
  community: Community;
  communityId: number;
}

export type CreateFine = Omit<Fine, 'community' | 'fineId'>;
export type UpdateFine = Omit<Fine, 'community'>;

export enum FineDenomination {
  CLP = 0,
  UF = 1,
  UTM = 2,
}

export interface AssignedFine {
  assignedFineId: number;
  fineId: number;
  unitId: number;
  eventDate: string;
  comment: string;
  fineName: string;
  amount: number;
  status: FineDenomination;
}

export type CreateAssignedFine = Omit<AssignedFine, 'assignedFineId' | 'fineName' | 'amount' | 'status'>;
export type UpdateAssignedFine = Omit<AssignedFine, 'fineName' | 'amount' | 'status'>;

export interface AssignFineData{
  assignedFineID: number;
  fineId: number;
  unitId: number;
  unitNumber: string;
  unitType: string;
  unitFloor: number;
  unitBuildingName: string;
  unitBuildingId : number;
  eventDate: string;
  comment: string;
  fineName: string;
  fineAmount: number;
  fineStatus: FineDenomination;
  user: string;
}
