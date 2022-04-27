export interface Faixa {
  faix_nr_sequencia?: string
  carg_nr_sequencia?: string
  depa_nr_sequencia?: string
  faix_tx_nome?: string
  faix_tx_descricao?: string
  faix_tx_tipo?: string
  faix_dt_vigencia_inicio?: Date
  faix_dt_vigencia_fim?: Date
  faix_tx_observacao?: string
  situ_tx_situacao?: string
  usua_nr_cadastro?: string
  usua_nr_edicao?: string
  data_dt_cadastro?: Date
  data_dt_edicao?: Date
  camposAuxiliares?: any
}