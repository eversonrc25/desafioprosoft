# TesteNetCore
# TesteNetCore


### dotnet new lib -o MiniFrameWork
### dotnet new webapi -o Segurancaapi
### dotnet new sln
### dotnet sln add MiniFrameWork/MiniFrameWork.csproj
### dotnet sln add Segurancaapi/Segurancaapi.csproj
### cd Segurancaapi
### dotnet add reference ../MiniFrameWork/MiniFrameWork.csproj

### dotnet add package EnterpriseLibrary.Common.NetCore
### dotnet add package EnterpriseLibrary.Data.NetCore
### dotnet add package Newtonsoft.Json

### http://localhost:5012/api/funcionalidade/?PAG_C=1&QTD_I=100


[
    {
        "title": "WorkFlow", "icon": "anchor", "type": "sub", "badgeType": "primary", "active": true, "nomeseguraca" :"WORK" ,
        "children" :
            [
                { "path": "/workflow/processofluxopauta", "title": "Painel do Processo Fluxo", "type": "link", "nomeseguraca" :"PRFL" },
                { "path": "/workflow/parametrizacaopauta", "title": "Parametrização do Fluxo", "type": "link", "nomeseguraca" :"PAPA" }
            ]
    },
    {
        "title": "Seguranca", "icon": "lock", "type": "sub", "badgeType": "primary", "active": false, "nomeseguraca" :"SEGU" ,
        "children":
            [
                { "path": "/seguranca/sistema", "title": "Sistema", "type": "link", "nomeseguraca" :"SIST" },
                { "path": "/seguranca/perfil" , "title": "Perfil" , "type": "link", "nomeseguraca" :"PERF" },
                { "path": "/seguranca/empresa" , "title": "Empresa" , "type": "link", "nomeseguraca" :"EMPR" },
                { "path": "/seguranca/sistemafuncionalidade" , "title": "Funcionalidades" , "type": "link", "nomeseguraca" :"FUNC" },
                { "path": "/seguranca/usuario" , "title": "Usuario" , "type": "link", "nomeseguraca" :"USUA" }              
            ]
    }
];