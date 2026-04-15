# SchemaMind – AI Schema Intelligence Assistant

## Overview

SchemaMind is a lightweight developer tool that combines database schema awareness with AI to generate accurate SQL queries using natural language.

Unlike typical AI SQL assistants that rely solely on prompts, SchemaMind builds structured context from database metadata before invoking AI.

---

## Problem

Most AI SQL tools follow:

Prompt → AI → SQL

This often leads to:

- incorrect joins  
- invalid column references  
- lack of schema awareness  
- no reuse of existing database logic  

---

## Solution

SchemaMind introduces Schema Intelligence:

Schema → Context → AI → Validation → Execution → Correction

Instead of asking AI to guess, SchemaMind provides structured schema context before generating SQL.

---

## Architecture

```
Client (VS Code Extension / Any UI) → SchemaMind API (.NET) → Schema Analyzer + Cache → Context Builder → AI Model (GitHub Models / OpenAI / Ollama) → SQL Validator → Query Executor → Self-Correction Loop → Results
```

---

## Features

- Schema-aware SQL generation  
- Join path discovery using foreign keys  
- Semantic table selection  
- Per-database schema caching (hashed connection keys)  
- AI self-correcting SQL execution loop (retry on failure)  
- Pluggable client interface (VS Code extension is one example, not mandatory)  

---

## How It Works

1. Developer enters a natural language query from any client (VS Code extension or other UI)  
2. Client sends query + connection string to API  
3. SchemaMind extracts tables and relationships (cached per DB)  
4. ContextBuilder prepares schema-aware prompt  
5. AI generates SQL  
6. SQL is validated for unsafe operations  
7. Query is executed  
8. If execution fails, error is sent back to AI for correction  
9. Process repeats (max 5 attempts)  
10. Final SQL and results are returned  

---

## Tech Stack

- .NET API  
- Microsoft.Extensions.AI  
- GitHub Models / OpenAI / Ollama  
- SQL Server / PostgreSQL  
- Dapper  
- IMemoryCache  
- VS Code Extension (example client implementation)  

---

## AI Model Integration

SchemaMind uses **Microsoft.Extensions.AI** to remain model-agnostic.

Supported integrations include:

- GitHub Models  
- Azure OpenAI / OpenAI  
- Local models (Ollama)  

This allows:

- easy switching between AI providers  
- flexibility in model experimentation  
- future extensibility without changing core logic  

---

## Repository Structure

```
SchemaMind/
├── BE/ → .NET API (Schema intelligence + AI pipeline)
└── FE/ → Example client (VS Code extension)
```

---

## API Example

### Request

```json
{
  "question": "Find customers with orders in the last 30 days",
  "connectionString": "Server=...;Database=...;Trusted_Connection=True;"
}
```

### Response

```json
{
  "query": "SELECT ...",
  "results": []
}
```

---

## Security Considerations (POC Scope)

- Only safe SQL operations allowed (no DROP, DELETE, ALTER, TRUNCATE)  
- Connection string passed per request (no persistence on server)  
- No credential storage in backend  
- Prompt-based systems may require additional validation in production  
- Query execution should be restricted to read-only operations in production  

---

## Design Decisions

- Stateless API using per-request connection string  
- Schema caching keyed by hashed connection string  
- Lightweight semantic table selection  
- Self-correcting AI loop instead of one-shot SQL generation  
- Separation of concerns across system components  
- Model abstraction using Microsoft.Extensions.AI  

---

## Future Enhancements

- Stored procedure and view reuse detection  
- Dependency and lineage analysis  
- Query explanation engine  
- Embedding-based semantic table selection  
- Multi-user isolation and authentication  
- Query sandboxing and execution limits  
- Support for multiple database providers  

---

## Why This Project

This project explores the intersection of:

- AI-assisted development  
- database schema intelligence  
- developer tooling  

It moves beyond simple prompt-based SQL generation into structured reasoning using database metadata.

---

## Note

This is a portfolio POC focused on architecture and workflow.  
Production-grade security, scalability, and user isolation are intentionally out of scope.
