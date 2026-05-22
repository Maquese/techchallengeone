# 🚗 Sistema Integrado de Atendimento e Execução de Serviços – Oficina Mecânica

## 📌 Objetivo
Este projeto é o MVP do back-end de um sistema para gestão de ordens de serviço, clientes, veículos e peças de uma oficina mecânica.  
O sistema foi desenvolvido aplicando **Domain-Driven Design (DDD)**, boas práticas de qualidade e segurança, e arquitetura em camadas.

---

## ⚙️ Funcionalidades
- Criação e acompanhamento de **Ordens de Serviço (OS)** com status automatizados:
  - Recebida, Em diagnóstico, Aguardando aprovação, Em execução, Finalizada, Entregue.
- Cadastro e gestão de **clientes, veículos, serviços e peças**.
- Controle de **estoque de peças e insumos**.
- Geração automática de **orçamentos** e envio para aprovação do cliente.
- Autenticação via **JWT** para APIs administrativas.
- APIs RESTful documentadas com **Swagger**.

---

## 🛠️ Tecnologias Utilizadas
- **Linguagem:** C#  
- **Framework:** ASP.NET Core  
- **Banco de Dados:** MySQL 8.0  
- **Containerização:** Docker + Docker Compose  
- **Testes:** Unitários e de integração com cobertura mínima de 80%

### 🎯 Justificativa da escolha do MySQL
O **MySQL** banco de dados relacional foi escolhido porque o sistema é simples e bem estruturado além que tem transações um pouco mais complexas que envolve algumas relações de dados e que precisam ser ACID porém no futuro existe a possibilidade de alguns micro serviços usarem outro tipo de banco de dados.

---

## 📂 Estrutura do Projeto
/Aplication
/AutoReparaAPI
/Domain
/Infra
/IOC
/Tests
Dockerfile
docker-compose.yml
GestaoAutoRepara.slnx
README.md

---

## 🚀 Como Executar Localmente
1. Clone o repositório:
   ```bash
   git clone https://github.com/Maquese/techchallengeone
2. Acesse a pasta 
cd TECHCHALLENGEONE
3. Suba os containers 
docker-compose up --build
4. Acesse a API 
http://localhost:5000
5. Doc Swagger 
http://localhost:5000/swagger
6. JWT
user: admin senha:admin123

---

## 🧪 Testes
dotnet test

---

## 🔒 Segurança
Autenticação JWT para rotas administrativas.
Validação de dados sensíveis (CPF, CNPJ, placa de veículo).

---


## 👥 Equipe
Kenney Maquese
Discord: Kenney - rm374177

---

## 📎 Links
📘 Documentação DDD : https://drive.google.com/file/d/1SiuB8-Hso8AXvtbeRIW2V1-Y8_mfmyWc/view?usp=sharing
📘 Toda a documentação: https://drive.google.com/drive/folders/17s-o27T-Lx22VP-ce8oVhZQR15ROc96a?usp=sharing

---

## Considerações 
Tive vários pontos que gostaria de fazer melhor mas me segurei devido a ser um MVP, segurança, estoque, cliente e outras coisas que devem 
ser melhoradas na continuidade do projeto.
Me debrucei muito mais no processo de entendimento do DDD que é o que de fato eu não tinha muito conhecimento e o que eu queria me aprofundar. 
Não manjo muito de OWASPZAP então pode não ter ficado tão bom quanto deveria, o meu processo foi chamar as api via insominia com proxy e depois atacar.
