export interface Reservation {
  userId: number;
  commonSpaceId: number;
  date: Date;
  startTime: Date;
  endTime: Date;
  status: ReservationStatus;
}

export enum ReservationStatus {
  PENDIENTE,
  CONFIRMADO,
  CANCELADO,
  FINALIZADA,
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
export type MyReservation = Pick<
  Reservation,
  'date' | 'startTime' | 'endTime' | 'status'
> & {
  spaceName: string;
  attendees: number;
  id: number;
  location: string;
};
