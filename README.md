# Movie Rental System - How I Solved the Problems

English version - Portuguese version below / Versão em português abaixo

Hi there! I received the movie rental system exercise and wanted to document here how I approached each problem I found in the code. I'll explain my thought process and the decisions I made during development.

## Problems I Found and How I Solved Them

When I first opened the project, I tried to run it and it wouldn't even start. I went investigating and found several problems that needed to be fixed.

**The startup error**

The application crashed when trying to start. I went to look at Program.cs and saw that you had registered IRentalFeatures as Singleton, but it depended on DbContext which is Scoped by default. This doesn't work because Singleton lives for the entire application but DbContext needs to be recreated per request to avoid concurrency issues and memory leaks.

I changed it to Scoped and also took the opportunity to add the missing registration for IMovieFeatures.

**The synchronous methods**

I noticed all methods were synchronous, which is a serious performance problem in a modern API. A synchronous database operation blocks the thread until it finishes, while the asynchronous version frees the thread to process other requests while waiting for I/O.

I converted all methods to async/await. This allows much more concurrent requests and is the recommended standard nowadays for .NET APIs.

**The incomplete GetRentalsByCustomerName method**

I saw the method was returning an empty list. I implemented the logic to filter by customer name, using case-insensitive LIKE and added Include to load related movie data. I also created the complete REST endpoint with proper validations.

**The CustomerName as string issue**

I noticed that having the customer name as a simple string in rental can create consistency problems. If we need more customer information or if there are changes, it becomes complicated to keep everything synchronized.

I created a Customer entity with Name, Email and CreatedAt. I configured the 1:N relationship between Customer and Rental.

**The problematic GetAll() method**

When I saw GetAll() loading all records into memory, I realized this would cause serious problems with lots of data. Without pagination, it can cause timeouts or even OutOfMemoryException.

I implemented mandatory pagination with sensible limits, used AsNoTracking() for read-only queries (better performance), created specific methods for different needs and added consistent ordering.

**Lack of exception handling**

The API had no error handling. Any unhandled exception would crash or return internal details to the client.

I created global middleware that catches all exceptions, returns standardized responses with appropriate HTTP codes, does structured logging for debugging and hides internal details for security.

## The Payment System Challenge

This part was interesting because it needed to be extensible. I used Strategy Pattern - created an IPaymentProvider interface and concrete implementations for each provider (PayPal, MbWay, CreditCard).

The most important thing was ensuring atomicity: if payment fails, the rental cannot be created. I used transactions for this - first process the payment, only save the rental if everything succeeds. If it fails, rollback and return error.

This allows adding new providers easily in the future without touching existing code.

## Extra Features I Implemented

To demonstrate broader knowledge, I implemented some things beyond what was asked:

I did complete CRUD for all entities with standardized REST endpoints. Added robust validations in multiple layers - Data Annotations on entities, business rules in services, constraints in database.

Created data seeding to facilitate development.

## What I Would Do Differently in a Larger Project

In a production application, I would implement some things I didn't put here due to time and scope:

**DTOs (Data Transfer Objects)** - I would completely separate the API from the domain model. This allows API versioning without impacting the domain, specific validations per endpoint, and better control over what is exposed externally.

**Logical layer separation** - I would divide into Controllers (coordination only), Services (business logic), Repositories (data abstraction), and Domain (entities and rules). This separation greatly facilitates scalability because each layer has well-defined responsibilities. For maintenance, it allows isolated changes - I can alter persistence without touching business logic.

**More abstractions** - I would create interfaces for repositories, implement Unit of Work pattern for better transaction management.

## Technical Decisions I Made

I chose Strategy Pattern for payments because it offers real extensibility - new providers without changing existing code, each provider can be tested in isolation, and changes remain localized.

Global middleware ensures all exceptions are handled consistently and improves security by hiding internal details.

I implemented async/await everywhere for better scalability, pagination to control memory, AsNoTracking() to optimize read-only queries, and connection pooling for efficient connection reuse.

## Final Reflection

The exercise allowed me to demonstrate not just technical knowledge, but ability to critically analyze existing code, careful refactoring while maintaining compatibility, and implementation of appropriate patterns.

I transformed a problematic project into a robust and scalable API following .NET best practices. Every decision was made thinking about quality, performance and ease of future maintenance.

## How to Run

```bash
git clone [repository-url]
cd MovieRental
dotnet restore
dotnet run
```

Access https://localhost:44321 to see the Swagger documentation.

I hope you can see my methodical approach and the architectural decisions I made. Any questions about specific implementations, I'm available to explain!

---

# Movie Rental System - Como Resolvi os Problemas



Olá! Recebi o exercício do sistema de aluguel de filmes e queria documentar aqui como abordei cada problema que encontrei no código. Vou explicar minha linha de raciocínio e as decisões que tomei durante o desenvolvimento.

## Problemas que Encontrei e Como Resolvi

Quando abri o projeto pela primeira vez, tentei executar e nem sequer iniciava. Fui investigar e encontrei vários problemas que precisavam ser resolvidos.

**O erro de inicialização**

A aplicação crashava ao tentar iniciar. Fui olhar o Program.cs e vi que vocês tinham registrado o IRentalFeatures como Singleton, mas ele dependia do DbContext que por padrão é Scoped. Isso não funciona porque o Singleton vive durante toda a aplicação mas o DbContext precisa ser recriado a cada request para evitar problemas de concorrência e vazamentos de memória.

Mudei para Scoped e também aproveitei para adicionar o registro do IMovieFeatures que estava faltando.

**Os métodos síncronos**

Notei que todos os métodos estavam síncronos, o que é um problema sério de performance numa API moderna. Uma operação de banco síncrona bloqueia a thread até terminar, enquanto a versão assíncrona liberta a thread para processar outras requests enquanto aguarda o I/O.

Converti todos os métodos para async/await. Isso permite muito mais requests simultâneas e é o padrão recomendado hoje em dia para APIs .NET.

**O método GetRentalsByCustomerName incompleto**

Vi que o método estava retornando uma lista vazia. Implementei a lógica para filtrar por nome do cliente, usando LIKE case-insensitive e adicionei Include para carregar os dados do filme relacionado. Também criei o endpoint REST completo com validações adequadas.

**A questão do CustomerName como string**

Notei que ter o nome do cliente como string simples no rental pode gerar problemas de consistência. Se precisarmos de mais informações do cliente ou se houver mudanças, fica complicado manter tudo sincronizado.

Criei uma entidade Customer com Name, Email e CreatedAt. Configurei o relacionamento 1:N entre Customer e Rental.

**O método GetAll() problemático**

Quando vi o GetAll() carregando todos os registros na memória, percebi que isso ia dar problemas sérios com muitos dados. Sem paginação, pode causar timeouts ou até OutOfMemoryException.

Implementei paginação obrigatória com limites sensatos, usei AsNoTracking() para queries read-only (melhor performance), criei métodos específicos para diferentes necessidades e adicionei ordenação consistente.

**Falta de tratamento de exceções**

A API não tinha nenhum tratamento de erros. Qualquer exception não tratada ia crashar ou retornar detalhes internos para o cliente.

Criei um middleware global que captura todas as exceções, retorna respostas padronizadas com códigos HTTP adequados, faz logging estruturado para debugging e oculta detalhes internos por segurança.

## O Challenge do Sistema de Pagamentos

Esta parte foi interessante porque precisava ser extensível. Usei o Strategy Pattern - criei uma interface IPaymentProvider e implementações concretas para cada provedor (PayPal, MbWay, CreditCard).

O mais importante foi garantir atomicidade: se o pagamento falha, o rental não pode ser criado. Usei transações para isso - primeiro processo o pagamento, só salvo o rental se der tudo certo. Se falhar, faz rollback e retorna erro.

Isso permite adicionar novos provedores facilmente no futuro sem mexer no código existente.

## Funcionalidades Extra que Implementei

Para demonstrar conhecimento mais amplo, implementei algumas coisas além do pedido:

Fiz CRUD completo para todas as entidades com endpoints REST padronizados. Adicionei validações robustas em múltiplas camadas - Data Annotations nas entidades, regras de negócio nos services, constraints no banco.

Criei seed de dados para facilitar o desenvolvimento.

## O que Faria Diferente num Projeto Maior

Numa aplicação de produção, eu implementaria algumas coisas que não coloquei aqui por questão de tempo e escopo:

**DTOs (Data Transfer Objects)** - Separaria completamente a API do modelo de domínio. Isso permite versionamento da API sem impactar o domínio, validações específicas por endpoint, e melhor controle sobre o que é exposto externamente.

**Separação em camadas lógicas** - Dividiria em Controllers (só coordenação), Services (lógica de negócio), Repositories (abstração de dados), e Domain (entidades e regras). Esta separação facilita muito a escalabilidade porque cada camada tem responsabilidades bem definidas. Para manutenção, permite mudanças isoladas - posso alterar a persistência sem tocar na lógica de negócio.

**Mais abstrações** - Criaria interfaces para os repositories, implementaria Unit of Work pattern para melhor gestão de transações.

## Decisões Técnicas que Tomei

Escolhi o Strategy Pattern para pagamentos porque oferece extensibilidade real - novos providers sem alterar código existente, cada provider pode ser testado isoladamente, e mudanças ficam localizadas.

O middleware global garante que todas as exceções sejam tratadas de forma consistente e melhora a segurança ocultando detalhes internos.

Implementei async/await em tudo para melhor escalabilidade, paginação para controlar memória, AsNoTracking() para otimizar queries read-only, e connection pooling para reutilização eficiente de conexões.

## Reflexão Final

O exercício me permitiu demonstrar não só conhecimento técnico, mas capacidade de análise crítica de código existente, refactoring cuidadoso mantendo compatibilidade, e implementação de padrões adequados.

Transformei um projeto com problemas numa API robusta e escalável seguindo as melhores práticas do .NET. Cada decisão foi pensada em qualidade, performance e facilidade de manutenção futura.

## Como Executar

```bash
git clone [repository-url]
cd MovieRental
dotnet restore
dotnet run
```

Acesse https://localhost:44321 para ver a documentação Swagger.

Espero que consigam ver minha abordagem metodológica e as decisões arquiteturais que tomei. Qualquer dúvida sobre alguma implementação específica, estou disponível para explicar!