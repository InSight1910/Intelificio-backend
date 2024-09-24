export interface Community {
  id?: number;
  address?: string;
  name?: string;
  adminName?: string;
  buildingCount?: number;
  unitCount?: number;
  municipalityId?: number;
  cityId?: number;
  regionId?: number;
}

export interface UsersCommunity {
  id: number;
  name: string;
  email: string;
  role: string;
  phoneNumber: string;
  unitCount: number;
}

export interface AllCommunity {
  id: number;
  name: string;
  address: string;
  adminName: string;
  creationDate: string;
  municipality: string;
  municipalityId: string;
  city: string;
  cityId?: string;
  region: string;
  regionId: string;
}
