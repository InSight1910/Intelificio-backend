export interface Attendee {
  id: number;
  name: string;
  email: string;
  rut: string;
}

export type AttendeeCreate = Omit<Attendee, 'id'> & { reservationId: number };
