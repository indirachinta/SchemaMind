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
