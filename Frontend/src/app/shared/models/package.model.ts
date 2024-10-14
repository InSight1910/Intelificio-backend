export interface Package {
  id: number;
  recipientName: string;
  conciergeName: string;
  deliveredToName: string;
  trackingNumber: string;
  status: PackageStatus;
  receptionDate: Date;
}

export enum PackageStatus {
  ENTREGADO,
  PENDIENTE,
}

export type CreatePackage = Omit<
  Package,
  | 'id'
  | 'receptionDate'
  | 'status'
  | 'recipientName'
  | 'conciergeName'
  | 'deliveredToName'
> & {
  communityId: number;
  recipientId: number;
  conciergeId: number;
  deliveredToId: number;
};