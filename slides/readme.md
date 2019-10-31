# Durable Functions

## Wer wir sind | Together

- Robert Meyer
  @roeb

- Thomas Freudenberg
  @thoemmi

DOMUS Software AG

---

## Microsoft Serverless World _(5min)_ | Together

- What the hack is serverless?

![Azure Function](images/evolution_of_cloud.png 'Cloud Evolution')

- Microsoft Serverless Plattform

![Azure Function](images/serverless_plattform.png 'Serverless Plattform')

- Azure Functions, Logic Apps, AppInsights, EventGrid (Serverless Infrastructure), Durable Functions, Serverless SQL Server,

---

## Azure Functions _(10min)_ | Thomas

![Azure Function](images/azure-functions.png 'Azure Function')

- Run small pieces of code serverless and short-term
- AppServicePlan vs Consumption Plan
- Flexibel Deployment
- Triggers and Bindings
  - Triggers
    - HTTP requests
    - Timer
    - Event Grid
    - Queues
    - ...
  - Bindings
    - CosmosDb
    - Table Storage
    - Queue Storage
- C#, F#, JavaScript, PowerShell, and Python

---

## Demo Azure Function _(5min)_ | Thomas

- Simple Functions in JS/Powershell and CS#

---

## Complex scenarios

![Function Chaining](images/function-chaining.png 'Function Chaining')

- Monitoring
- Where am I?

---

## Durable Functions _(20min)_ | Robert

- Extension to Azure Function - Durable Task Framework
- C#, F#, and JavaScript
- How to start a orchestration?
  - `DurableOrchestrationClient` in `HttpTrigger` oder `ServiceBusTrigger` Function
  - Explain InstanceId
- Orchestration functions
- Activity functions
- Replay Pattern inside Durable Functions
- Track workflow from start to completion / Stateful (Azure Storage)
- Patterns

  - Function chaining
  - Fan out/in
  - HTTP Async Response
  - React to external events (Human interaction)
  - Actors

- Timeout handling
- Dehydration/Hydration
- Durable REST API

---

## Demo Durable Functions _(5min)_ | Robert

Simple workflow (Aprovall Workflow)

---

## Demo Durable Functions _(10min)_ | Robert

- More complex workflow w/ C# _(Should we show VA Workflow here?)_
- Durable Dungeons

---

## Durable Functions 2.0 _(10min)_ | Robert

- Durable Entities
  - Durable Entities REST-API
- Interface implementierung from IOrchestrationClient, IOrchestrationContext, ...
  | Alter Typ | Neuer Typ |
  | ------------------------------- | ---------------------------- |
  | DurableOrchestrationClientBase | IDurableOrchestrationClient |
  | DurableOrchestrationContextBase | IDurableOrchestrationContext |
  | DurableActivityContextBase | IDurableActivityContext |
  | OrchestrationClientAttribute | DurableClientAttribute |
- Durable HTTP (Call from HTTP-APIs directly in the orchestration)
- Alternate storage providers

  - Azure Service Bus
  - In-Memory Emulator
  - Redis

- DEMO: Durable Entities -> Simple Approval WF with Durable Entities

---

## Pains _(10min+)_ | Together

- How to fail with serverless
- KeepAlive for normal functions
- Monitoring
- Versioning
- ...
