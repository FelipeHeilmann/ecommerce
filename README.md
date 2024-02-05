# Projeto E-Comerve

Este projeto é um sistema de e-comerce que é desenvolvido utilizando .NET 8, CQRS, Mediator, EF Core, SQL Server e MongoDB, DDD e seguindo a arquitetura limpa (Clean Architecture). O sistema é distribuído em microsserviços, adotando uma arquitetura orientada a eventos e utiliza o RabbitMQ como sistema de mensageria.

## Microsserviços

### Order Service
O serviço de pedidos é responsável pela autenticação do usuário e pela criação de pedidos.

### Payment Service
O serviço de pagamento lida com o processamento dos pagamentos para os pedidos.

### Invoice System
O sistema de faturas cuida da geração e gerenciamento das Notas Fiscais para os pedidos.

### Notification System
O sistema de notificações trata do envio de notificações aos usuários, principalmente via e-mail.

### Stock System
O sistema de controle de estoque dos produtos.

## Tecnologias Utilizadas

- **.NET:** A estrutura principal do desenvolvimento.
- **CQRS:** Padrão de projeto para separar as responsabilidades de leitura e escrita.
- **Mediator:** Implementação do padrão de design mediator para facilitar a comunicação entre componentes.
- **RabbitMQ:** Sistema de mensageria entre os microsserviços.
- **EF Core:** Framework de acesso a dados para interação com o banco de dados.
- **SQL Server** Banco de dados relacional para a persistência dos dados.
- **MongoDB** Banco de dados não relacional para facilitar a velocidade da leitura dos dados.
- **DDD (Domain-Driven Design):** Abordagem de design de software que enfatiza a colaboração entre especialistas no domínio e desenvolvedores para criar um modelo que reflete o mundo real.
- **Arquitetura Limpa (Clean Architecture):** Metodologia de design de software que preza pela separação de responsabilidades e independência das camadas.



