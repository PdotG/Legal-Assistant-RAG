-- init.sql

-- Create pgvector extension if it does not exist
CREATE EXTENSION IF NOT EXISTS vector;

-- Table for Users
CREATE TABLE users (
    id_user SERIAL PRIMARY KEY,
    name TEXT NOT NULL,
    email TEXT NOT NULL UNIQUE,
    password TEXT NOT NULL
);

-- Table for Files
CREATE TABLE files (
    id_file SERIAL PRIMARY KEY,
    id_user INTEGER NOT NULL REFERENCES users(id_user) ON DELETE CASCADE,
    name TEXT NOT NULL,
    content bytea NOT NULL,
    scraped_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table for Embeddings
CREATE TABLE embeddings (
    id_embedding SERIAL PRIMARY KEY,
    id_file INTEGER NOT NULL REFERENCES files(id_file) ON DELETE CASCADE,
    embedding vector,
    chunk_index INTEGER,
    plain_text TEXT,
    embedding_created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
