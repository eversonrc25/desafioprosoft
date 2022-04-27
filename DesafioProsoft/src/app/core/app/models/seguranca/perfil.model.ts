export interface Perfil {
  perf_nr_sequencia?: string
  perf_tx_descricao?: string
  situ_tx_situacao?: string
  usua_nr_cadastro?: string
  usua_nr_edicao?: string
  data_dt_cadastro?: Date
  data_dt_edicao?: Date
  camposAuxiliares?: any
  AcoesPerfil?: Acoes[];
}

export interface  Acoes {
  perf_nr_sequencia?:string;
  fuac_nr_sequencia?:string;
  func_nr_sequencia?:string;
  acaobanco?:string;

}