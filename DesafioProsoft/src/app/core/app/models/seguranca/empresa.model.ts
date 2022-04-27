export interface Empresa {
  empr_nr_sequencia?: string
  empr_tx_nome?: string
  empr_tx_razao_social?: string
  empr_tx_cnpj?: string
  empr_tx_logradouro?: string
  empr_tx_numero?: string
  empr_tx_bairro?: string
  empr_tx_municipio?: string
  empr_tx_uf?: string
  empr_tx_cep?: string
  empr_tx_telefone?: string
  empr_tx_email?: string
  empr_tx_bd?: string
  empr_tx_esquema?: string
  empr_tx_senha?: string
  situ_tx_situacao?: string
  usua_nr_cadastro?: string
  usua_nr_edicao?: string
  data_dt_cadastro?: Date
  data_dt_edicao?: Date
  camposAuxiliares?: any
}