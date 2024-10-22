export interface Expense {
  name: string;
  amount: string;
  date: Date;
  type: ExpenseType;
  providerRut: string;
  invoice: string;
  pucharseOrder?: string;
  communityId: number;
}

export enum ExpenseType {
  Administrative = 'Administrativo',
  UsageAndConsumption = 'Uso y Consumo',
  Maintainance = 'Mantenimiento',
  Repair = 'Reparación',
  Urgent = 'Urgente',
  NonProratable = 'No prorrateable',
}
