export interface CommonSpace {
  id: number;
  name: string;
  capacity: number;
  location: string;
  isInMaintenance: boolean;
  communityId: number;
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
  isInMaintenance?: boolean;
  communityId?: number;
}