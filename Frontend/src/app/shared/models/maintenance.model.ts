export interface Maintenance {
  id?: number;
  startDate: string;
  endDate: string;
  comment: string;
  commondSpaceId: number;
  communityId: number;
  commonSpaceName: string;
  commonSpaceLocation: string;
  isActive: boolean;
}
