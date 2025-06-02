-- Создание пользователя (если ещё не создан)
DO
$$
BEGIN
   IF NOT EXISTS (
       SELECT FROM pg_catalog.pg_roles
       WHERE rolname = 'bloguser') THEN

       CREATE USER bloguser WITH PASSWORD 'bloguser';
   END IF;
END
$$;

-- Переключаемся на нужную БД
\connect blogdb

-- Создание таблицы (если ещё не создана)
CREATE TABLE IF NOT EXISTS blog (
    id serial PRIMARY KEY,
    title VARCHAR(50) NOT NULL,
    description VARCHAR(100) NOT NULL
);

-- Назначение владельца
ALTER TABLE blog OWNER TO bloguser;

-- Вставка тестовых данных (без дубликатов)
INSERT INTO blog (title, description)
SELECT 'Title 1', 'Description 1'
WHERE NOT EXISTS (SELECT 1 FROM blog WHERE id = 1);

INSERT INTO blog (title, description)
SELECT 'Title 2', 'Description 2'
WHERE NOT EXISTS (SELECT 1 FROM blog WHERE id = 2);

INSERT INTO blog (title, description)
SELECT 'Title 3', 'Description 3'
WHERE NOT EXISTS (SELECT 1 FROM blog WHERE id = 3);

INSERT INTO blog (title, description)
SELECT 'Title 4', 'Description 4'
WHERE NOT EXISTS (SELECT 1 FROM blog WHERE id = 4);