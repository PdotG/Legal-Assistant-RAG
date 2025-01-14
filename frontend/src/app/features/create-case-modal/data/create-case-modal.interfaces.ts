export interface CaseSummaryDto {
    idCase: number;
    title: string;
    status: string;
}

export interface CaseResponseDto {
    idCase: number;
    title: string;
    description?: string;
    status: string;
    createdDate: string;
    updatedDate: string;
    courtDate?: string;
    client?: ClientSummaryDto;
    assignedUser?: UserSummaryDto;
    documents?: DocumentSummaryDto[];
}

export interface CaseRequestDto {
    title: string;
    description?: string;
    status: string;
    courtDate?: string;
    clientId: number;
    assignedUserId: number;
}

export interface ClientSummaryDto {
    idClient: number;
    idUser: number;
    name: string;
}

export interface UserSummaryDto {
    idUser: number;
    name: string;
}

export interface DocumentSummaryDto {
    idDocument: number;
    title: string;
    uploadedDate: string;
}