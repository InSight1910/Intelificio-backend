export interface Reservation {
  userId: number;
  commonSpaceId: number;
  date: Date;
  startTime: Date;
  endTime: Date;
  status: ReservationStatus;
}

export enum ReservationStatus {
  CONFIRMADO,
  CANCELADO,
  FINALIZADA,
  PENDIENTE,
}
export interface CountReservation {
  day: number;
  countReservations: {
    status: ReservationStatus;
    count: number;
  }[];
}
export type CreateReservation = Omit<Reservation, 'status'>;
export type ListReservation = Omit<Reservation, 'userId' | 'commonSpaceId'> & {
  userName: string;
  spaceName: string;
};
