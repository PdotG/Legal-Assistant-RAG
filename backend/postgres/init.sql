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
    file_path TEXT NOT NULL,
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

-- Table for Clients
CREATE TABLE clients (
    id_client SERIAL PRIMARY KEY,
    id_user INTEGER NOT NULL REFERENCES users(id_user) ON DELETE CASCADE,
    name TEXT NOT NULL,
    contact_information TEXT NOT NULL,
    address TEXT,
    notes TEXT
);

-- Table for Cases
CREATE TABLE cases (
    id_case SERIAL PRIMARY KEY,
    title TEXT NOT NULL,
    description TEXT,
    status TEXT CHECK (status IN ('Open', 'Closed', 'Pending')) DEFAULT 'Open',
    assigned_user_id INTEGER REFERENCES users(id_user) ON DELETE SET NULL,
    client_id INTEGER NOT NULL REFERENCES clients(id_client) ON DELETE CASCADE,
    created_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    court_date TIMESTAMP WITH TIME ZONE
);

-- Table for Documents
CREATE TABLE documents (
    id_document SERIAL PRIMARY KEY,
    id_case INTEGER NOT NULL REFERENCES cases(id_case) ON DELETE CASCADE,
    title TEXT NOT NULL,
    description TEXT,
    file_id INTEGER NOT NULL REFERENCES files(id_file) ON DELETE CASCADE,
    uploaded_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
