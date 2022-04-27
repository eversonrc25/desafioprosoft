export interface RetornoApi<T> {
  error?: boolean;
  mensagem?: string;
  dados?: T;
  total_registro?: number;
}
