# itau-desafio

##  Como rodar o projeto

### Requisitos

- .NET 8
- Node.js
- NPM
- Banco de dados já populado com pelo menos **um e-mail válido cadastrado** para que o front-end funcione corretamente durante os testes.


Instale as dependências:
npm install

Inicie o projeto front-end:
npm run dev

obs: Iniciar também a API.
---

# Controle de Investimentos - Documentação Técnica

## Tabela de Conteúdos

- [Script SQL da criação das tabelas](#script-sql-da-criação-das-tabelas)
- [Justificativa dos tipos de dados usados](#justificativa-dos-tipos-de-dados-usados)
- [Consulta-Alvo Otimizada e Índice](#consulta-alvo-otimizada-e-índice)
- [Teste Mutante e Sua Importância](#teste-mutante-e-sua-importância)
- [Escalabilidade e Performance](#escalabilidade-e-performance-do-sistema-de-investimentos)
- [Endpoints RESTful e OpenAPI](#endpoint-restful-método-url-parâmetros--documentação-em-formato-openapi-30-yaml)
- [Observações sobre a Documentação](#observações-sobre-a-documentação-openAPI-30)

---

## Script SQL da criação das tabelas:

```sql
CREATE DATABASE InvestmentControlDb;
```

```sql
CREATE TABLE usuarios (
id INT AUTO_INCREMENT PRIMARY KEY,
nome VARCHAR(100) NOT NULL,
email VARCHAR(100) NOT NULL UNIQUE,
percentual_corretagem DECIMAL(5,2) NOT NULL
);
```

```sql
CREATE TABLE ativos (
id INT AUTO_INCREMENT PRIMARY KEY,
codigo VARCHAR(10) NOT NULL UNIQUE,
nome VARCHAR(100) NOT NULL
);
```

```sql
CREATE TABLE operacoes (
id INT AUTO_INCREMENT PRIMARY KEY,
usuario_id INT NOT NULL,
ativo_id INT NOT NULL,
quantidade INT NOT NULL,
preco_unitario DECIMAL(15,4) NOT NULL,
tipo_operacao ENUM('compra', 'venda') NOT NULL,
corretagem DECIMAL(10,2) NOT NULL,
data_hora DATETIME NOT NULL,
FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
FOREIGN KEY (ativo_id) REFERENCES ativos(id)
);
```

```sql
CREATE TABLE cotacoes (
id INT AUTO_INCREMENT PRIMARY KEY,
ativo_id INT NOT NULL,
preco_unitario DECIMAL(15,4) NOT NULL,
data_hora DATETIME NOT NULL,
FOREIGN KEY (ativo_id) REFERENCES ativos(id)
);
```

```sql
CREATE TABLE posicoes (
id INT AUTO_INCREMENT PRIMARY KEY,
usuario_id INT NOT NULL,
ativo_id INT NOT NULL,
quantidade INT NOT NULL,
preco_medio DECIMAL(15,4) NOT NULL,
pl DECIMAL(15,2) NOT NULL,
FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
FOREIGN KEY (ativo_id) REFERENCES ativos(id),
UNIQUE (usuario_id, ativo_id)
);
```

---

## Justificativa dos tipos de dados usados:

#### IDs com `INT AUTO_INCREMENT`:

- Usado em todas as tabelas para os campos `id`, porque é um tipo de dado eficiente para chaves primárias que representam identificadores numéricos.
- É um tipo de dado que suporta valores grandes (±2 bilhões) e garante unicidade com `AUTO_INCREMENT`**_–_** o qual faz cada novo registro receber um ID único e sequencial automaticamente.

#### Nomes e emails com `VARCHAR(100)`:

- Ideal para armazenar dados de texto (strings) de tamanho variável.
- Economiza espaço, pois armazena apenas os caracteres necessários.
- Permite nomes e email de até 100 caracteres, que costuma ser suficiente.
- `UNIQUE` em `usuarios.email`e `ativos.codigo` evita duplicatas.

#### `DECIMAL(5,2)` para `percentual_corretagem`:

- `DECIMAL` é um tipo de dado muito bom para armazenar valores monetários e percentuais que exigem precisão exata, como 1.25%.
- `5,2` = significa um número até 5 dígitos, sendo 2 dígitos após o ponto decimal (ex: 123.45).

#### Códigos de Ativo com `VARCHAR(10)`:

- É adequado porque a maioria dos códigos de ativos (ex: PETR4, ITUB3) tem no máximo 6 caracteres e são alfanuméricos.

#### `INT` para `quantidade`:

- Quantidades de ativos são sempre inteiros (Ex: 100 ações, 1000 cotas, etc), então **`INT`** é a escolha certa.
- É mais leve e direto que `DECIMAL`.

#### `DECIMAL(15,4)` para preços e `preco_medio`:

- Preços de ativos podem ter várias casas decimais para representar centavos, o que exige precisão.
- `DECIMAL(15,4)` permite até 15 dígitos no total, com 4 casas decimais (ex: 12345678901.2345).

#### `ENUM('compra', 'venda')` para `tipo_operacao`:

- Ideal para campos que têm um conjunto predefinido e limitado de valores possíveis.
- Restringe as opções a "compra" ou "venda" para garantir consistência de dados.

#### `DECIMAL(10,2)` para `corretagem`:

- Corretagens são valores monetários com 2 casas decimais e `DECIMAL(10,2)` permite valores de até 10 dígitos, com 2 casas decimais (ex: 12345678.90).
- Suporta corretagens de até 99 milhões (bem além do necessário).

#### `DATETIME` para `data_hora`:

- Armazena data e hora exata (ano, mês, dia, hora, minuto, segundo).
- Muito bom para registrar o momento exato em que uma operação ou cotação ocorreu, permitindo análises temporais precisas.

#### `DECIMAL(15,2)` para `pl` (Profit & Loss = Lucro & Prejuízo):

- Armazena valores com até 15 dígitos e 2 casas após a vírgula.
- Bom para representar lucros e perdas, sejam eles positivos ou negativos.

#### `UNIQUE (usuario_id, ativo_id)` na tabela `posicoes`:

- Garante que cada usuário só tenha uma posição agregada por ativo.
- Evita duplicidade no controle de investimentos.

---

## Consulta-Alvo Otimizada e Índice

#### Consulta-Alvo Otimizada:

```sql
SELECT *
FROM operacoes
WHERE usuario_id = [ID_DO_USUARIO]
  AND data_hora >= DATE_SUB(CURDATE(), INTERVAL 30 DAY);
```

#### Índice Proposto:

```sql
CREATE INDEX idx_operacoes_usuario_ativo_data
ON operacoes (usuario_id, ativo_id, data_hora);
```

#### Justificativa Técnica do Índice:

1. **`usuario_id` (1º campo do índice):**

   - É a coluna mais seletiva da consulta.
   - Vem primeiro no WHERE, então colocar ela como primeira no índice composto permite ao banco localizar rapidamente as operações só daquele usuário.

2. **`data_hora` (3º campo do índice):**


    - Permite navegação eficiente por data dentro do  conjunto de operações do usuário.
    - A combinação ```usuario_id + data_hora``` ajuda a buscar  diretamente as operações dentro do intervalo dos   últimos 30 dias, sem precisar escanear toda a tabela.

3. **`ativo_id` (2º campo do índice)**

  - Não é usada diretamente nessa consulta, mas:
    - Melhora a seletividade do índice em outras consultas.
    - Pode transformar a query em “coberta” se ela estiver buscando só essas 3 colunas — evitando ida extra à tabela (melhora de performance real).

#### Comparações com Outros Índices Possíveis:

| **Índice Alternativo** | Por que não é ideal |
| --- | --- |
| `usuario_id` sozinho | Precisa varrer todas as datas depois |
| `data_hora` sozinho | Varreria a tabela inteira e só depois filtraria por usuário |
| `data_hora`, `usuario_id` | Pega todas as datas e só então filtra por usuário — inversão da lógica da consulta |

---

## Teste Mutante e Sua Importância

#### O que é Teste Mutante (_Mutation Testing_)?

- É uma técnica avançada de teste de software usada para avaliar a qualidade dos testes existentes.
- O que ele faz:
  - Introduzir pequenas e intencionais alterações (mutações) no código-fonte de um programa, criando "mutantes". _Cada mutante é uma versão levemente modificada do código original._
  - O objetivo é que cada mutante seja "morto". Então, se espera que os testes **falhem**, indicando que detectaram o erro.
- Se o teste **não falhar**, o mutante **sobrevive**. O que indica uma falha no conjunto de testes atuais.

As mutações são tipicamente pequenas e representam erros comuns que os programadores podem cometer, como:

- Trocar operadores aritméticos (`+` por `-`, `*` por `/`).
- Trocar operadores relacionais (`>` por `>=`, `==` por `!=`)
- Remover ou duplicar linhas de código.
- Mudar valores booleanos (`true` para `false`).
- Remover chamadas de método.--

#### Qual a Importância do Teste Mutante?

| Vantagem | Explicação |
|---------|------------|
| **Avalia a eficácia dos testes** | Vai além da cobertura de linhas — mostra se os testes realmente **detectam falhas**. |
| **Mostra pontos cegos** | Se o mutante passa, há uma **lacuna no teste**. |
| **Eleva a confiabilidade do código** | Testes precisam ser **fortes o suficiente** pra detectar erros sutis. |
| **Estimula bom design de código** | Código difícil de testar geralmente tem **baixa taxa de mutantes mortos** e precisa de **refatoração**. |
| **Reduz dependência de testes manuais** | Quanto mais mutantes mortos, mais **confiança nos testes automáticos**. |

---

#### Exemplo de Mutação Aplicada ao Método de Preço Médio

O cálculo do preço médio é fundamental para determinar o custo de aquisição e calcular o lucro ou prejuízo de um ativo.

 **Código original**

```csharp
total_custo += op['quantidade'] * op['preco_unitario'];
```

**Código mutante**

```csharp
total_custo += op['quantidade'] + op['preco_unitario'];
```

**Conclusão**

A falha do teste demonstra sua eficácia: o teste conseguiu "matar" o mutante, detectando uma alteração sutil, porém significativa, no comportamento do código. Se o teste não tivesse falhado (e o mutante tivesse "sobrevivido"), isso indicaria uma deficiência no conjunto de testes.

---

## Escalabilidade e Performance do Sistema de Investimentos

#### Situação hipotética:

Com o crescimento do sistema, o volume de operações subiu para 1 milhão/dia.

- Explique como aplicar auto-scaling horizontal no serviço

- Compare estratégias de balanceamento de carga (round-robin vs latência). 

#### 1. Auto-scaling Horizontal Inteligente:

Para lidar com a variabilidade e os picos de carga, a aplicação de auto-scaling horizontal é crucial.

Esta estratégia permite que o sistema adicione ou remova automaticamente instâncias de serviço (sejam servidores, contêineres ou pods) em tempo real, baseando-se em métricas de desempenho.

A monitorização contínua de indicadores como uso de CPU, latência de resposta, volume de requisições por segundo (RPS) e a capacidade da fila de mensagens permitiria a definição de gatilhos precisos para o escalonamento. 

Tecnologias de orquestração como o Kubernetes (com seu Horizontal Pod Autoscaler - HPA) ou serviços de Auto Scaling em provedores de nuvem seriam ferramentas ideais para implementar essa elasticidade, garantindo que o sistema sempre tenha recursos adequados sem desperdício.

#### 2. Balanceamento de Carga Otimizado por Latência:

Para a distribuição eficiente das requisições entre as instâncias escaladas, uma estratégia de balanceamento de carga é fundamental. 

Em um sistema de investimentos, onde a baixa latência é crítica para a execução de ordens e a experiência do usuário, a escolha ideal seria o algoritmo de "Menor Latência" (Least Latency).

Diferente do Round-Robin, que distribui sequencialmente sem considerar a carga, o balanceamento por menor latência direciona as novas requisições para o servidor mais rápido e responsivo no momento

Isso garante que o tráfego seja sempre otimizado, evitando gargalos em servidores sobrecarregados e proporcionando a melhor performance possível, especialmente em momentos de alta volatilidade do mercado.

---

## Endpoint RESTful (método, URL, parâmetros) + documentação em formato OpenAPI 3.0 (YAML)

### 1. Permitir informar um ativo e receber a última cotação

- **Método HTTP:** `GET`
- **URL:** `/quotes/{assetSymbol}/latest`
- **Propósito:** Obter a última cotação de um ativo específico.
- **Parâmetros:**
    - `assetSymbol` (parâmetro de caminho): O símbolo do ativo (ex: `PETR4`, `VALE3`).

**Exemplo de requisição:**

`GET /quotes/PETR4/latest`

**Exemplo de resposta (JSON):**

```json
{
  "assetSymbol": "PETR4",
  "price": 35.75,
  "timestamp": "2025-06-09T18:00:00Z"
}
```

---

### 2. Consultar o preço médio por ativo para um usuário

- **Método HTTP:** `GET`
- **URL:** `/users/{userId}/assets/{assetSymbol}/average-price`
- **Propósito:** Obter o preço médio de compra de um ativo específico para um determinado usuário.
- **Parâmetros:**
    - `userId` (parâmetro de caminho): O ID único do usuário/cliente.
    - `assetSymbol` (parâmetro de caminho): O símbolo do ativo.

**Exemplo de requisição:**

`GET /users/CLIENTE123/assets/PETR4/average-price`

**Exemplo de resposta (JSON):**

```json
{
  "userId": "CLIENTE123",
  "assetSymbol": "PETR4",
  "averagePrice": 32.10
}
```

---

### 3. Consultar a posição de um cliente

- **Método HTTP:** `GET`
- **URL:** `/clients/{clientId}/positions`
- **Propósito:** Obter a posição (todos os ativos e suas quantidades) de um cliente específico.
- **Parâmetros:**
    - `clientId` (parâmetro de caminho): O ID único do cliente.

**Exemplo de requisição:**

`GET /clients/CLIENTE123/positions`

**Exemplo de resposta (JSON):**

```json
{
  "clientId": "CLIENTE123",
  "totalInvested": 150000.00,
  "currentValue": 155000.00,
  "assets": [
    {
      "assetSymbol": "PETR4",
      "quantity": 1000,
      "currentPrice": 35.75,
      "totalValue": 35750.00
    },
    {
      "assetSymbol": "VALE3",
      "quantity": 500,
      "currentPrice": 68.20,
      "totalValue": 34100.00
    }
  ]
}
```

---

### 4. Ver o valor financeiro ganho pela corretora com as corretagens

- **Método HTTP:** `GET`
- **URL:** `/brokerage/fees/total`
- **Propósito:** Obter o valor total das corretagens recebidas pela corretora em um período.
- **Parâmetros (Query Parameters):**
    - `startDate` (opcional): Data de início para o filtro (formato `YYYY-MM-DD`).
    - `endDate` (opcional): Data de fim para o filtro (formato `YYYY-MM-DD`).

**Exemplo de requisição:**

`GET /brokerage/fees/total?startDate=2025-01-01&endDate=2025-06-30`

**Exemplo de resposta (JSON):**

```json
{
  "totalBrokerageFees": 25000.50,
  "currency": "BRL",
  "periodStart": "2025-01-01",
  "periodEnd": "2025-06-30"
}
```

---

### 5. Receber os Top 10 clientes com maiores posições, e os Top 10 clientes que mais pagaram corretagem

- **Método HTTP:** `GET`
- **URL:** `/clients/rankings`
- **Propósito:** Obter rankings dos clientes por posição e corretagem paga.

**Exemplo de requisição:**

`GET /clients/rankings`

**Exemplo de resposta (JSON):**

```json
{
  "top10ByPosition": [
    {
      "clientId": "CLIENTE001",
      "name": "Cliente Alfa",
      "totalPositionValue": 500000.00
    },
    {
      "clientId": "CLIENTE002",
      "name": "Cliente Beta",
      "totalPositionValue": 450000.00
    }
    // ... até 10 clientes
  ],
  "top10ByBrokerageFees": [
    {
      "clientId": "CLIENTE005",
      "name": "Cliente Gama",
      "totalBrokeragePaid": 1500.00
    },
    {
      "clientId": "CLIENTE001",
      "name": "Cliente Alfa",
      "totalBrokeragePaid": 1200.00
    }
    // ... até 10 clientes
  ]
}
```

---

### Documentação OpenAPI 3.0 (YAML)

```yaml
openapi: 3.0.0
info:
  title: API da Corretora de Investimentos
  description: API para consulta de cotações, posições de clientes, e dados de corretagem.
  version: 1.0.0
servers:
  - url: https://api.corretora.com.br/v1
    description: Servidor de Produção
  - url: http://localhost:8080/v1
    description: Servidor de Desenvolvimento
tags:
  - name: Cotações
    description: Operações relacionadas às cotações de ativos.
  - name: Usuários e Clientes
    description: Operações relacionadas a dados de usuários e clientes.
  - name: Corretora
    description: Operações relacionadas a dados financeiros da corretora.
paths:
  /quotes/{assetSymbol}/latest:
    get:
      tags:
        - Cotações
      summary: Obter a última cotação de um ativo
      operationId: getLatestQuote
      description: Retorna a cotação mais recente para o símbolo do ativo especificado.
      parameters:
        - name: assetSymbol
          in: path
          required: true
          description: O símbolo do ativo (ex: PETR4, VALE3).
          schema:
            type: string
            example: PETR4
      responses:
        '200':
          description: Última cotação do ativo retornada com sucesso.
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/LatestQuote'
              examples:
                success:
                  value:
                    assetSymbol: "PETR4"
                    price: 35.75
                    timestamp: "2025-06-09T18:00:00Z"
        '404':
          description: Ativo não encontrado.
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'
              examples:
                notFound:
                  value:
                    code: "ASSET_NOT_FOUND"
                    message: "O ativo com o símbolo especificado não foi encontrado."
        '500':
          description: Erro interno do servidor.
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'
  /users/{userId}/assets/{assetSymbol}/average-price:
    get:
      tags:
        - Usuários e Clientes
      summary: Consultar preço médio por ativo para um usuário
      operationId: getUserAssetAveragePrice
      description: Retorna o preço médio de compra de um ativo específico para um determinado usuário.
      parameters:
        - name: userId
          in: path
          required: true
          description: O ID único do usuário/cliente.
          schema:
            type: string
            format: uuid # Assumindo UUID para IDs de usuário
            example: "a1b2c3d4-e5f6-7890-1234-567890abcdef"
        - name: assetSymbol
          in: path
          required: true
          description: O símbolo do ativo.
          schema:
            type: string
            example: VALE3
      responses:
        '200':
          description: Preço médio do ativo para o usuário retornado com sucesso.
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/UserAssetAveragePrice'
              examples:
                success:
                  value:
                    userId: "a1b2c3d4-e5f6-7890-1234-567890abcdef"
                    assetSymbol: "PETR4"
                    averagePrice: 32.10
        '404':
          description: Usuário ou ativo não encontrado para o usuário.
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'
              examples:
                userOrAssetNotFound:
                  value:
                    code: "USER_OR_ASSET_NOT_FOUND"
                    message: "Usuário ou ativo não encontrado para o ID e símbolo especificados."
        '500':
          description: Erro interno do servidor.
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'
  /clients/{clientId}/positions:
    get:
      tags:
        - Usuários e Clientes
      summary: Consultar a posição de um cliente
      operationId: getClientPositions
      description: Retorna a posição completa (todos os ativos e suas quantidades) de um cliente específico.
      parameters:
        - name: clientId
          in: path
          required: true
          description: O ID único do cliente.
          schema:
            type: string
            format: uuid
            example: "c1d2e3f4-g5h6-7890-5678-90abcdef1234"
      responses:
        '200':
          description: Posição do cliente retornada com sucesso.
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ClientPosition'
              examples:
                success:
                  value:
                    clientId: "c1d2e3f4-g5h6-7890-5678-90abcdef1234"
                    totalInvested: 150000.00
                    currentValue: 155000.00
                    assets:
                      - assetSymbol: "PETR4"
                        quantity: 1000
                        currentPrice: 35.75
                        totalValue: 35750.00
                      - assetSymbol: "VALE3"
                        quantity: 500
                        currentPrice: 68.20
                        totalValue: 34100.00
        '404':
          description: Cliente não encontrado.
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'
              examples:
                clientNotFound:
                  value:
                    code: "CLIENT_NOT_FOUND"
                    message: "O cliente com o ID especificado não foi encontrado."
        '500':
          description: Erro interno do servidor.
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'
  /brokerage/fees/total:
    get:
      tags:
        - Corretora
      summary: Ver o valor financeiro ganho pela corretora com as corretagens
      operationId: getTotalBrokerageFees
      description: Retorna o valor total das corretagens recebidas pela corretora, opcionalmente filtrado por um período.
      parameters:
        - name: startDate
          in: query
          required: false
          description: Data de início para o filtro (formato YYYY-MM-DD).
          schema:
            type: string
            format: date
            example: "2025-01-01"
        - name: endDate
          in: query
          required: false
          description: Data de fim para o filtro (formato YYYY-MM-DD).
          schema:
            type: string
            format: date
            example: "2025-06-30"
      responses:
        '200':
          description: Valor total das corretagens retornado com sucesso.
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/TotalBrokerageFees'
              examples:
                success:
                  value:
                    totalBrokerageFees: 25000.50
                    currency: "BRL"
                    periodStart: "2025-01-01"
                    periodEnd: "2025-06-30"
        '400':
          description: Parâmetros de data inválidos.
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'
              examples:
                invalidDates:
                  value:
                    code: "INVALID_DATE_RANGE"
                    message: "As datas de início e fim fornecidas são inválidas ou o período é incorreto."
        '500':
          description: Erro interno do servidor.
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'
  /clients/rankings:
    get:
      tags:
        - Usuários e Clientes
      summary: Obter rankings de clientes (top posições e top corretagens)
      operationId: getClientRankings
      description: Retorna os Top 10 clientes com maiores posições e os Top 10 clientes que mais pagaram corretagem.
      responses:
        '200':
          description: Rankings de clientes retornados com sucesso.
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ClientRankings'
              examples:
                success:
                  value:
                    top10ByPosition:
                      - clientId: "CLIENTE001"
                        name: "Cliente Alfa"
                        totalPositionValue: 500000.00
                      - clientId: "CLIENTE002"
                        name: "Cliente Beta"
                        totalPositionValue: 450000.00
                    top10ByBrokerageFees:
                      - clientId: "CLIENTE005"
                        name: "Cliente Gama"
                        totalBrokeragePaid: 1500.00
                      - clientId: "CLIENTE001"
                        name: "Cliente Alfa"
                        totalBrokeragePaid: 1200.00
        '500':
          description: Erro interno do servidor.
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'

components:
  schemas:
    LatestQuote:
      type: object
      properties:
        assetSymbol:
          type: string
          description: O símbolo do ativo.
          example: "PETR4"
        price:
          type: number
          format: float
          description: A última cotação do ativo.
          example: 35.75
        timestamp:
          type: string
          format: date-time
          description: Data e hora da cotação.
          example: "2025-06-09T18:00:00Z"
      required:
        - assetSymbol
        - price
        - timestamp

    UserAssetAveragePrice:
      type: object
      properties:
        userId:
          type: string
          format: uuid
          description: O ID único do usuário.
          example: "a1b2c3d4-e5f6-7890-1234-567890abcdef"
        assetSymbol:
          type: string
          description: O símbolo do ativo.
          example: "PETR4"
        averagePrice:
          type: number
          format: float
          description: O preço médio de compra do ativo para o usuário.
          example: 32.10
      required:
        - userId
        - assetSymbol
        - averagePrice

    ClientPosition:
      type: object
      properties:
        clientId:
          type: string
          format: uuid
          description: O ID único do cliente.
          example: "c1d2e3f4-g5h6-7890-5678-90abcdef1234"
        totalInvested:
          type: number
          format: float
          description: Valor total investido pelo cliente em todos os ativos.
          example: 150000.00
        currentValue:
          type: number
          format: float
          description: Valor atual total da posição do cliente.
          example: 155000.00
        assets:
          type: array
          description: Lista de ativos na posição do cliente.
          items:
            $ref: '#/components/schemas/AssetPosition'
      required:
        - clientId
        - totalInvested
        - currentValue
        - assets

    AssetPosition:
      type: object
      properties:
        assetSymbol:
          type: string
          description: O símbolo do ativo.
          example: "PETR4"
        quantity:
          type: integer
          description: A quantidade do ativo possuída.
          example: 1000
        currentPrice:
          type: number
          format: float
          description: A última cotação do ativo.
          example: 35.75
        totalValue:
          type: number
          format: float
          description: O valor total atual desta posição.
          example: 35750.00
      required:
        - assetSymbol
        - quantity
        - currentPrice
        - totalValue

    TotalBrokerageFees:
      type: object
      properties:
        totalBrokerageFees:
          type: number
          format: float
          description: O valor total das corretagens ganhas pela corretora.
          example: 25000.50
        currency:
          type: string
          description: A moeda das corretagens.
          example: "BRL"
        periodStart:
          type: string
          format: date
          description: Data de início do período consultado.
          example: "2025-01-01"
        periodEnd:
          type: string
          format: date
          description: Data de fim do período consultado.
          example: "2025-06-30"
      required:
        - totalBrokerageFees
        - currency

    ClientRankings:
      type: object
      properties:
        top10ByPosition:
          type: array
          description: Lista dos Top 10 clientes com maiores posições.
          items:
            $ref: '#/components/schemas/RankedClientPosition'
        top10ByBrokerageFees:
          type: array
          description: Lista dos Top 10 clientes que mais pagaram corretagem.
          items:
            $ref: '#/components/schemas/RankedClientBrokerage'
      required:
        - top10ByPosition
        - top10ByBrokerageFees

    RankedClientPosition:
      type: object
      properties:
        clientId:
          type: string
          format: uuid
          description: O ID único do cliente.
          example: "CLIENTE001"
        name:
          type: string
          description: O nome do cliente.
          example: "Cliente Alfa"
        totalPositionValue:
          type: number
          format: float
          description: O valor total da posição do cliente.
          example: 500000.00
      required:
        - clientId
        - name
        - totalPositionValue

    RankedClientBrokerage:
      type: object
      properties:
        clientId:
          type: string
          format: uuid
          description: O ID único do cliente.
          example: "CLIENTE005"
        name:
          type: string
          description: O nome do cliente.
          example: "Cliente Gama"
        totalBrokeragePaid:
          type: number
          format: float
          description: O valor total de corretagens pagas pelo cliente.
          example: 1500.00
      required:
        - clientId
        - name
        - totalBrokeragePaid

    ErrorResponse:
      type: object
      properties:
        code:
          type: string
          description: Um código de erro específico para a situação.
          example: "INVALID_DATE_RANGE"
        message:
          type: string
          description: Uma mensagem descritiva do erro.
          example: "As datas de início e fim fornecidas são inválidas ou o período é incorreto."
      required:
        - code
        - message
```

---

## Observações sobre a Documentação OpenAPI 3.0

- **`tags`**: Ajuda a organizar os endpoints em grupos lógicos na documentação gerada (ex: Swagger UI).
- **`format`**: Usado para dar mais granularidade ao tipo de dado (ex: `uuid` para IDs, `date-time` para timestamps, `float` para números decimais).
- **`examples`**: Essencial para mostrar como as requisições e respostas devem ser, facilitando o entendimento para os desenvolvedores que consumirão a API.
- **`$ref`**: Permite reutilizar definições de esquemas (`schemas`) para evitar repetição e manter a consistência.
- **Códigos de Status HTTP**: Foram incluídos códigos de status comuns (200, 400, 404, 500) com descrições e esquemas de erro genéricos (`ErrorResponse`).
- **Segurança**: Para uma API real, seria necessário adicionar seções de segurança (por exemplo, autenticação com OAuth2 ou API Keys) à documentação OpenAPI, usando o campo `securitySchemes` e aplicando-os aos `paths`. Isso não foi incluído para manter o foco nos requisitos solicitados.

---
