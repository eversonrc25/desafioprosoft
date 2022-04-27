export interface AcoesPerfil {
  sist_nr_sequencia?: string;
  sist_tx_descricao?: string;
  func_nr_sequencia?: string;
  func_tx_descricao?: string;
  func_tx_nome?: string;
  fuac_nr_sequencia?: string;
  pefu_nr_sequencia?: string;
  perf_nr_sequencia?: string;
  fuac_tx_descricao?: string;
  fuac_tx_nome?: string;
  ehAssociado?: string;
};


 
  export interface PerfilSistemaFuncionalidadeAcoes {
      fuac_nr_sequencia: string;
      fuac_tx_descricao: string;
      fuac_tx_nome: string;
      ehAssociado: string;
      pefu_nr_sequencia: string;
      perf_nr_sequencia: string;
  };

  export interface PerfilSistemaFuncionalidade {
      func_nr_sequencia: string;
      func_tx_descricao: string;
      func_tx_nome: string;
      acoes: PerfilSistemaFuncionalidadeAcoes[];
  };

  export interface PerfilSistema {
      sist_nr_sequencia?: string;
      sist_tx_descricao?: string;
      funcionalidades?: PerfilSistemaFuncionalidade[];
      camposAuxiliares?: string;
  };

 

