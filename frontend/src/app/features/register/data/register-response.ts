export interface RegisterSuccessResponse {
    message: string;
  }
  
  export interface RegisterConflictResponse {
    message: string;
  }
  
  export interface RegisterErrorResponse {
    message: string;
    details?: string;
  }