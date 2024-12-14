export interface Client {
    idClient: number;
    idUser: number;
    name: string;
    contactInformation: string;
    address?: string;
    notes?: string;
    cases?: CaseSummary[];
  }
  
  export interface CaseSummary {
    idCase: number;
    title: string;
    status: string;
  }