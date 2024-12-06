import { User } from './user';

export interface File {
  id: number;
  userId: number;
  user?: User;
  name: string;
  scrapedAt: Date;
  // Aqui en el backend el modelo implementa dos parametros mas
  // "Filepath" y "embeddings" no se si hace falta implementar
}
