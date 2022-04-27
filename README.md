## Projetos

Antes de executar a aplicação ,excutar o script exportar.sql para criação da base, a senha para o usuario sa deve ser subistitudio no aqrquivo appsettings.json e appsettings.Development.json na linha bancosSQL, na aplicação DesafioProsoft-api

### Framwork
Projeto de do framework em net core


### apigatway
Projeto de gateway responsável por chamar as outras api's

```bash
cd apigateway 
dotnet watch run
```
### seguranca-api
Projeto de responsável pela api da seguranca 

```bash
cd seguranca-api/Segurancaapi
dotnet watch run
```

### DesafioProsoft-api
Projeto de responsável pela manipulação dos dados

```bash
cd DesafioProsoft-api/DesafioProsoftapi
dotnet watch run
```

### FrontEnd
Projeto de frontend

```bash
cd DesadioProsoft
npm install
ng serve
```

