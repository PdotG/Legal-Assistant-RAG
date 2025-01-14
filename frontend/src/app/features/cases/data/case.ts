export interface Case {
  idCase: number;
  title: string;
  description?: string;
  status: string;
  courtDate?: Date;
  client: ClientSummary;
}

export interface ClientSummary {
  idClient: number;
  name: string;
}