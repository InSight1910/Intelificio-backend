export interface Community {
  id?: number;
  address?: string;
  name?: string;
  adminName?: string;
  adminId?: number;
  rut?: string;
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
  adminId: number;
  creationDate: string;
  municipality: string;
  municipalityId: string;
  city: string;
  cityId?: string;
  region: string;
  regionId: string;
}

export interface UserAdmin {
  id: number;
  fullName: string;
  phoneNumber: string;
  email: string;
  rut: string;
}

export interface CreateCommunity {
  name: string;
  rut: string;
  address: string;
  municipalityId: number;
}
