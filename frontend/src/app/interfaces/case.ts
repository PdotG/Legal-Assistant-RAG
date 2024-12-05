import { Client } from './client';
import { User } from './user';

export interface Case {
  id: number;
  title: string;
  description?: string;
  createdDate: Date;
  updatedDate: Date;
  courtDate: Date;
  clientId: number;
  client?: Client;
  assugnedUserId: number;
  assignedUser?: User;
  //falta implementar public ICollection<Document>? Documents { get; set; }
}
