API de Gestão de Tarefas (ToDo API)
Esta é uma API RESTful desenvolvida em .NET 8 que simula a gestão de tarefas, permitindo operações de criação, listagem, atualização e remoção de tarefas.

🛠 Funcionalidades
Criar Tarefas: Adiciona uma nova tarefa ao sistema.

Listar Tarefas: Retorna todas as tarefas, com opções de filtragem por status e data de vencimento.

Atualizar Tarefas: Modifica uma tarefa existente.

Remover Tarefas: Exclui uma tarefa pelo seu ID.

Estrutura da Tarefa
Cada tarefa possui os seguintes campos:

Id (int): Identificador único da tarefa.

Titulo (string): Título da tarefa (obrigatório, máximo 100 caracteres).

Descricao (string): Descrição detalhada da tarefa (opcional, máximo 500 caracteres).

Status (enum StatusTarefa): Estado atual da tarefa (Pendente, EmAndamento, Concluido). Padrão: Pendente.

DataVencimento (DateTime): Data e hora de vencimento da tarefa (obrigatório).

🚀 Como Rodar a Aplicação
A aplicação é configurada para rodar utilizando Docker Compose, o que simplifica a configuração do ambiente, incluindo o banco de dados SQL Server. As migrações do banco de dados são um passo manual.

Pré-requisitos
Docker Desktop (ou Docker Engine + Docker Compose) instalado e em execução em sua máquina.

Download Docker Desktop

SDK do .NET 8 (necessário para rodar as migrações manualmente e os testes unitários).

Download .NET SDK

Ferramentas Entity Framework Core CLI (dotnet-ef) instaladas globalmente (necessário para aplicar migrações manualmente).

dotnet tool install --global dotnet-ef
# Se já tiver, pode atualizar:
# dotnet tool update --global dotnet-ef

1. Clonar o Repositório
git clone <URL_DO_SEU_REPOSITORIO>
cd GestaoDeTarefasAPI # Navegue para a pasta do projeto que contém o docker-compose.yml

2. Gerar Migrações (Primeira Vez ou Após Alterações no Modelo)
Importante: Este passo é necessário se você estiver criando o projeto pela primeira vez ou se fizer alterações no modelo de dados (Tarefa.cs, ToDoContext.cs).

Certifique-se de que a pasta GestaoDeTarefasAPI/Data/Migrations está vazia ou não existe antes de gerar novas migrações.

# Na pasta GestaoDeTarefasAPI/ (onde está o .csproj)
dotnet ef migrations add InitialCreateSqlServer -c ToDoContext -o Data/Migrations

3. Iniciar os Serviços Docker (Banco de Dados e API)
Navegue até o diretório GestaoDeTarefasAPI/ (onde o docker-compose.yml e o Dockerfile estão) no seu terminal.

Execute o seguinte comando para construir as imagens e iniciar os contêineres do SQL Server e da sua API:

docker-compose up --build -d

A flag --build garante que suas imagens sejam construídas a partir do Dockerfile mais recente.

A flag -d executa os contêineres em segundo plano (detached mode).

Você pode verificar o status dos contêineres com:

docker ps

4. Aplicar Migrações ao Banco de Dados (Passo Manual)
CRUCIAL: Após os contêineres iniciarem (docker ps deve mostrar o sqlserver como (healthy)), você precisa aplicar as migrações do banco de dados. Este comando será executado na sua máquina host, mas se conectará ao SQL Server que está rodando no Docker.

# Na pasta GestaoDeTarefasAPI/ (onde está o .csproj)
dotnet ef database update --connection "Server=localhost,1433;Database=GestaoDeTarefasDB;User Id=sa;Password=teste@123;TrustServerCertificate=True;Command Timeout=60;"

Explicação da String de Conexão:

Server=localhost,1433: Conecta-se à porta 1433 da sua máquina local, que é mapeada para a porta 1433 do contêiner SQL Server.

Database=GestaoDeTarefasDB: Nome do banco de dados (certifique-se que o nome do seu DbContext usa o mesmo).

User Id=sa;Password=teste@123: Credenciais para o SQL Server no Docker.

TrustServerCertificate=True: Necessário para certificados autoassinados em ambientes de desenvolvimento.

Command Timeout=60: Dá 60 segundos para o comando ser executado.

Você pode ver os logs da sua API para verificar se não há erros de conexão após as migrações serem aplicadas:

docker logs gestaodetarefasapi-gestaodetarefasapi-1

5. Acessar a API
Uma vez que os contêineres estejam rodando e as migrações aplicadas, sua API estará acessível via Swagger UI:

URL da Swagger UI: http://localhost:8080/swagger

Abra seu navegador e teste os endpoints (Criar, Listar, Atualizar, Remover tarefas).

6. Parar a Aplicação
Para parar e remover todos os contêineres, redes e volumes criados pelo Docker Compose:

docker-compose down -v

A flag -v (ou --volumes) garante que os volumes persistentes (incluindo os dados do SQL Server) sejam removidos. Use com cautela se quiser manter seus dados entre as sessões.

🧪 Executando Testes Unitários
Para rodar os testes unitários da aplicação:

Navegue até a raiz da sua solução no terminal.

Execute o comando:

dotnet test

🏗 Estrutura do Projeto
.
├── GestaoDeTarefasAPI/
│   ├── Controllers/
│   │   └── TarefaController.cs
│   ├── Data/
│   │   ├── ToDoContext.cs
│   │   └── Migrations/  # Pasta gerada pelo Entity Framework Core para migrações
│   │       └── ...
│   ├── Models/
│   │   └── Tarefa.cs
│   ├── Repositories/
│   │   ├── ITarefaRepository.cs
│   │   └── TarefaRepository.cs
│   │   └── IUnitOfWork.cs
│   │   └── UnitOfWork.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── GestaoDeTarefasAPI.csproj
│   ├── Dockerfile            # Dockerfile para build da aplicação
│   ├── docker-compose.yml    # Orquestração Docker para API e SQL Server 
│   └── Program.cs
├── GestaoDeTarefasAPI.Tests/
│   ├── TarefaRepositoryTests.cs
│   └── GestaoDeTarefasAPI.Tests.csproj
└── GestaoDeTarefasAPI.sln    # Arquivo de solução (UM NÍVEL ACIMA DA PASTA DA API)