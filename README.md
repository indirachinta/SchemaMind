# SchemaMind – AI Schema Intelligence Assistant

## 🚀 Overview

SchemaMind is a lightweight developer tool that combines database schema awareness with AI to generate accurate SQL queries using natural language.

Unlike typical AI SQL assistants that rely solely on prompts, SchemaMind builds structured context from database metadata before invoking AI.

---

## 💡 Problem

Most AI SQL tools follow:

Prompt → AI → SQL

This often leads to:

- incorrect joins  
- invalid column references  
- lack of schema awareness  
- no reuse of existing database logic  

---

## 🧠 Solution

SchemaMind introduces **Schema Intelligence**:

Schema → Context → AI → Validation → Execution → Correction

Instead of asking AI to guess, SchemaMind provides structured schema context before generating SQL.

---

## ⚙️ Architecture
VS Code Extension
↓
SchemaMind API (.NET)
↓
Schema Analyzer + Cache
↓
Context Builder
↓
AI Model (GitHub / OpenAI)
↓
SQL Validator
↓
Query Executor
↓
Self-Correction Loop
↓
Results



---

## 🔑 Features

- Schema-aware SQL generation  
- Join path discovery using foreign keys  
- Semantic table selection  
- Per-database schema caching (hashed connection keys)  
- AI self-correcting SQL execution loop (retry on failure)  
- VS Code extension integration  

---

## 🔄 How It Works

1. Developer enters a natural language query in VS Code  
2. Extension sends query + connection string to API  
3. SchemaMind extracts tables and relationships (cached per DB)  
4. ContextBuilder prepares schema-aware prompt  
5. AI generates SQL  
6. SQL is validated for unsafe operations  
7. Query is executed  
8. If execution fails → error is sent back to AI for correction  
9. Process repeats (max 5 attempts)  
10. Final SQL + results returned  

---

## 🧰 Tech Stack

- .NET API  
- Microsoft.Extensions.AI  
- SQL Server / PostgreSQL  
- Dapper  
- IMemoryCache  
- VS Code Extension (TypeScript)  

---

## 📦 API Example

### Request

```json
{
  "question": "Find customers with orders in the last 30 days",
  "connectionString": "Server=...;Database=...;Trusted_Connection=True;"
}

### Response
{
  "query": "SELECT ...",
  "results": []
}
Security Considerations (POC Scope)
Only safe SQL operations allowed (no DROP, DELETE, ALTER, TRUNCATE)
Connection string passed per request (no persistence on server)
No credential storage in backend
Prompt-based systems may be vulnerable to injection → requires validation and sandboxing in production
Query execution should be restricted to read-only operations in production environments


Design Decisions
Stateless API using per-request connection string
Schema caching keyed by hashed connection string
Lightweight semantic table selection for improved accuracy
Self-correcting AI loop instead of one-shot SQL generation
Separation of concerns across schema extraction, context building, AI reasoning, and execution layers

Future Enhancements
Stored procedure / view reuse detection
Dependency and lineage analysis
Query explanation for legacy SQL
Embedding-based semantic table selection
Multi-user isolation and authentication
Query sandboxing and execution limits
Support for multiple database providers

Repo Structure
SchemaMind/
 ├── BE/   → .NET API (Schema intelligence + AI pipeline)
 ├── FE/   → VS Code extension (developer interface)

Note
This is a portfolio POC focused on architecture and workflow.
Production-grade security, scalability, and user isolation are intentionally out of scope.
