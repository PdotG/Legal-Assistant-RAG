import { Client } from './client';
import { User } from './user';
import { Document } from './document';

export interface Case {
  id: number;
  title: string;
  description?: string;
  status: string;
  createdDate: Date;
  updatedDate: Date;
  courtDate: Date;
  clientId: number;
  client?: Client;
  assignedUserId: number;
  assignedUser?: User;
  documents?: Document[];
}
