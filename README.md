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

```sql
CREATE INDEX idx_operacoes_usuario_ativo_data
ON operacoes (usuario_id, ativo_id, data_hora);
```

```sql
CREATE INDEX idx_cotacoes_ativo_data
ON cotacoes (ativo_id, data_hora DESC);
```

```sql
CREATE UNIQUE INDEX idx_posicoes_usuario_ativo
ON posicoes (usuario_id, ativo_id);
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

### Endpoints RESTful

| #  | Método | URL                                         | Parâmetros/Body                                                                 | Descrição                                      |
|----|--------|---------------------------------------------|---------------------------------------------------------------------------------|------------------------------------------------|
| 1  | GET    | /api/cotacao/ativo/{ativoId}                | ativoId (int, obrigatório, path)                                                | Obter a cotação atual de um ativo               |
| 2  | GET    | /api/operacao/preco-medio                   | -                                                                               | Consultar o preço médio geral das operações     |
| 3  | GET    | /api/operacao/corretora/faturamento         | -                                                                               | Consultar faturamento da corretora              |
| 4  | POST   | /api/operacao                              | Body: { usuarioId, ativoId, quantidade, precoUnitario, tipoOperacao, corretagem } | Registrar nova operação                         |
| 5  | GET    | /api/posicao/classificacao                  | -                                                                               | Obter classificações de posições                |
| 6  | GET    | /api/posicao/usuario/{usuarioId}            | usuarioId (int, obrigatório, path)                                              | Obter posições de um usuário                    |
| 7  | GET    | /api/usuario/{usuarioId}/operacoes          | usuarioId (int, obrigatório, path)                                              | Obter operações de um usuário                   |
| 8  | GET    | /api/usuario/{usuarioId}/global             | usuarioId (int, obrigatório, path)                                              | Obter visão global da carteira de um usuário    |
| 9  | GET    | /api/usuario/{usuarioId}/corretagem         | usuarioId (int, obrigatório, path)                                              | Obter total de corretagens pagas por usuário    |
| 10 | GET    | /api/usuario/{usuarioId}/cotacoes           | usuarioId (int, obrigatório, path)                                              | Obter cotações dos ativos de um usuário         |
| 11 | GET    | /api/usuario/{email}                        | email (string, obrigatório, path)                                               | Buscar usuário por e-mail                       |

---

### Documentação OpenAPI 3.0 (YAML) - Exemplo

```yaml
openapi: 3.0.1
info:
  title: InvestmentControl.API
  version: "1.0"
paths:
  /api/cotacao/ativo/{ativoId}:
    get:
      tags: [Cotacoes]
      summary: Obter cotação atual de um ativo
      parameters:
        - name: ativoId
          in: path
          required: true
          schema:
            type: integer
      responses:
        '200':
          description: Cotação retornada com sucesso

  /api/operacao/preco-medio:
    get:
      tags: [Operacoes]
      summary: Consultar preço médio geral
      responses:
        '200':
          description: Preço médio retornado com sucesso

  /api/operacao/corretora/faturamento:
    get:
      tags: [Operacoes]
      summary: Consultar faturamento total da corretora
      responses:
        '200':
          description: Valor total de corretagens retornado

  /api/operacao:
    post:
      tags: [Operacoes]
      summary: Registrar nova operação
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/PostOperacaoCommand'
      responses:
        '200':
          description: Operação registrada com sucesso

  /api/posicao/classificacao:
    get:
      tags: [Posicoes]
      summary: Obter classificações de posições
      responses:
        '200':
          description: Lista de classificações retornada

  /api/posicao/usuario/{usuarioId}:
    get:
      tags: [Posicoes]
      summary: Obter posições do usuário
      parameters:
        - name: usuarioId
          in: path
          required: true
          schema:
            type: integer
      responses:
        '200':
          description: Lista de posições retornada

  /api/usuario/{usuarioId}/operacoes:
    get:
      tags: [Usuarios]
      summary: Obter operações de um usuário
      parameters:
        - name: usuarioId
          in: path
          required: true
          schema:
            type: integer
      responses:
        '200':
          description: Operações retornadas

  /api/usuario/{usuarioId}/global:
    get:
      tags: [Usuarios]
      summary: Obter visão global da carteira
      parameters:
        - name: usuarioId
          in: path
          required: true
          schema:
            type: integer
      responses:
        '200':
          description: Visão global retornada

  /api/usuario/{usuarioId}/corretagem:
    get:
      tags: [Usuarios]
      summary: Total de corretagens pagas
      parameters:
        - name: usuarioId
          in: path
          required: true
          schema:
            type: integer
      responses:
        '200':
          description: Valor total de corretagens retornado

  /api/usuario/{usuarioId}/cotacoes:
    get:
      tags: [Usuarios]
      summary: Cotações dos ativos do usuário
      parameters:
        - name: usuarioId
          in: path
          required: true
          schema:
            type: integer
      responses:
        '200':
          description: Lista de cotações retornada

  /api/usuario/{email}:
    get:
      tags: [Usuarios]
      summary: Buscar usuário por e-mail
      parameters:
        - name: email
          in: path
          required: true
          schema:
            type: string
      responses:
        '200':
          description: Usuário retornado com sucesso

components:
  schemas:
    PostOperacaoCommand:
      type: object
      properties:
        usuarioId:
          type: integer
        ativoId:
          type: integer
        quantidade:
          type: integer
        precoUnitario:
          type: number
          format: double
        tipoOperacao:
          type: integer
          enum: [1, 2]
        corretagem:
          type: number
          format: double
      required:
        - usuarioId
        - ativoId
        - quantidade
        - precoUnitario
        - tipoOperacao
        - corretagem

```

---
