import { Case } from './case';

export interface Client {
  id: number;
  name: string;
  contactInformation: string;
  address?: string;
  notes?: string;
  Cases?: Case[];
}
