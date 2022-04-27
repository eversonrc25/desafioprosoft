export interface User {
    nome: string;
    apelido: string;
    email: string;
    id_usuario: number;
    accessToken: string;
    created: Date;
    exp: Date;
    cdversao: string;
    ambiente: string;    
    routes: string[];
    roles: string[];
    tamanho_arquivos: number;
}
