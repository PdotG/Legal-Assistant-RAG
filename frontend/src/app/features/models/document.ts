import { Case } from './case';

export interface Document {
  id: number;
  title: string;
  description?: string;
  uploadDate: Date;
  caseId: number;
  case?: Case;
  fileId: number;
  // falta implementar backend.models.File ? File
}
