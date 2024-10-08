export interface CommonSpace {
  id: number;
  name: string;
  capacity: number;
  location: string;
  isInMaintenance: boolean;
  communityId: number;
  startDate?: string;
  endDate?: string;
  comment?: string;
}
export interface CreateCommonSpace {
  name: string;
  capacity: number;
  location: string;
  isInMaintenance: boolean;
  communityId: number;
}

export interface UpdateCommonSpace {
  name?: string;
  capacity?: number;
  location?: string;
  IsInMaintenance?: boolean;
  startDate?: string;
  endDate?: string;
  comment?: string;
}
