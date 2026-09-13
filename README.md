# Projeto: Auto-repara-API

## 1. Objetivo do projeto
Descreva aqui o propósito do repositório e o problema que ele resolve.

Exemplo:
- Gerenciar ordens de serviço de uma oficina mecânica.
- Controlar clientes, veículos, serviços, estoque e orçamento.
- Expor APIs para integração com frontend e outros sistemas.

---

## 2. Tecnologias utilizadas
- .NET 10
- C#
- Entity Framework
- Docker
- Kubernetes
- Terraform
- Swagger / OpenAPI
- GitHub Actions
- Testes com  NUnit

---

## 3. Arquitetura e componentes

### Visão geral
O sistema é composto pelos seguintes módulos:
- API
- Application
- Domain
- Infra
- IOC
- Tests

### Diagrama de componentes
![Diagrama de componentes - ](images/componentes.png)


### Fluxo principal
1. Cliente realiza requisição na API.
2. A API valida a request e direciona para o caso de uso.
3. A camada de aplicação executa a regra de negócio.
4. A infraestrutura persiste ou consulta os dados.
5. A resposta é devolvida em JSON para o cliente.

---

## 4. Estrutura do repositório
```text
/
├── src/
│   ├── AutoReparaAPI/
│   ├── Application/
│   ├── Domain/
│   ├── Infra/
│   ├── IOC/
│   └── Tests/
├── infra/
│   └── main.tf
├── k8s/
│   ├── deployment.yaml
│   ├── service.yaml
│   └── ...
├── Dockerfile
├── docker-compose.yml
├── README.md
└── ...
```

---

## 5. Pré-requisitos
Antes de rodar o projeto, verifique se você possui:
- .NET SDK instalado
- Docker e Docker Compose instalados
- Kubernetes / Minikube / Kind (se for deploy local em cluster)
- Terraform (se a infraestrutura for provisionada via IaC)
- Banco de dados configurado

---

## 6. Execução local

A aplicação pode ser executada localmente sem qualquer publicação no Docker Hub. O Docker Hub é apenas o repositório da imagem para uso em outros ambientes e no Kubernetes.

### Opção 1: via Docker Compose
```bash
cd src

docker compose up --build
```

Acesse:
- API: http://localhost:<porta>
- Swagger: http://localhost:<porta>/swagger
- OpenAPI: http://localhost:<porta>/openapi/v1.json

### Opção 2: via .NET local
```bash
dotnet restore
dotnet build
dotnet run --project src/AutoReparaAPI/AutoReparaAPI.csproj
```

### Build local da imagem Docker
```bash
docker build -t auto-repara-api:local .
```

### Publicação no Docker Hub
Para publicar a imagem no Docker Hub, substitua o nome conforme sua conta:

```bash
docker build -t SEU_USUARIO/auto-repara-api:latest .
docker login
docker push SEU_USUARIO/auto-repara-api:latest
```

> Altere apenas os valores `SEU_USUARIO` e `auto-repara-api` de acordo com seu usuário e nome do repositório no Docker Hub.

### Onde a pessoa precisa alterar a imagem
Existem dois pontos principais que normalmente precisam ser ajustados:

1. No comando de build/push:
```bash
docker build -t SEU_USUARIO/auto-repara-api:latest .
```

2. No manifesto do Kubernetes:
```yaml
image: SEU_USUARIO/auto-repara-api:latest
```

Se o projeto usar `docker-compose` com imagem fixa, também pode haver ajuste no campo `image` desse arquivo.

> Em resumo: rodar localmente não depende do Docker Hub; o Docker Hub é apenas para publicar a imagem para uso em outros ambientes, como Kubernetes, homologação ou produção.

---

## 7. Deploy

### Deploy local com Docker
Sobe uma imagem docker dentro do ambiente local.

### Deploy em Kubernetes
Sobe um pod num cluster Kubernetes.

### Deploy docker-hub
Sobe a imagem e disponibiliza no repo do docker-hub

> Dockerfile deve existir apenas quando for necessário para build da imagem de aplicação em Kubernetes ou containerização real.

---

## 8. Pipeline de CI/CD

### Visão geral
A pipeline executa os seguintes passos:
1. Checkout do código
2. Restore dos pacotes
3. Build da solução
4. Execução dos testes
5. Publicação da imagem Docker (se aplicável)
6. Deploy em ambiente de homologação / produção

### Fluxo da pipeline
- build
- test
- docker build
- push image
- deploy

---

## 9. Documentação da API
### Swagger
- URL local: http://localhost:<porta>/swagger

### Insominia
- Collection: https://drive.google.com/file/d/1qeOqcmxKIwq-32-0sxej9vbQJo34ZpdQ/view?usp=drive_link
- Workspace: New Environment aws (copy) 

---

## 10. Observabilidade e logs
- Logs estruturados em JSON
- Correlação por request
- Métricas e alertas conforme ambiente

---

## 11. Testes
```bash
dotnet test
```

Cobertura esperada:
- testes unitários
- testes de integração
- validação de regras de negócio

---

