export interface Case {
  idCase: number;
  title: string;
  description?: string;
  status: string;
  courtDate?: Date;
  client: ClientSummary;
  // ...otros campos relevantes...
}

export interface ClientSummary {
  idClient: number;
  name: string;
}