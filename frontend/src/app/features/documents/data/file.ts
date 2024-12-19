export interface FileUploadDto {
  id: number;
  name: string;
  file: File;
  filePath: string;
  scrapedAt: Date;
}
