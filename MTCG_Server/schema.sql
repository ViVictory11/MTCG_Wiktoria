-- Drop tables if they already exist (cascade ensures dependent objects are also removed)
DROP TABLE IF EXISTS battlelog CASCADE;
DROP TABLE IF EXISTS battle CASCADE;
DROP TABLE IF EXISTS stack CASCADE;
DROP TABLE IF EXISTS cards CASCADE;
DROP TABLE IF EXISTS users CASCADE;

-- Users table
CREATE TABLE users
(
    id       SERIAL PRIMARY KEY,           -- Auto-incrementing primary key
    username VARCHAR(50)  NOT NULL UNIQUE, -- Unique username
    password VARCHAR(255) NOT NULL         -- Password (hashed)
);

-- Cards table
CREATE TABLE cards
(
    id      SERIAL PRIMARY KEY,     -- Auto-incrementing primary key
    name    VARCHAR(100)  NOT NULL, -- Name of the card
    damage  NUMERIC(5, 2) NOT NULL, -- Damage value with up to 2 decimal places
    element VARCHAR(50)   NOT NULL, -- Element type (e.g., Fire, Water)
    type    VARCHAR(50)   NOT NULL, -- Card type (e.g., Monster, Spell)
    species VARCHAR(50)   NOT NULL  -- Card species (e.g., Dragon, Elf)
);

-- Deck table
CREATE TABLE stack
(
    id           SERIAL PRIMARY KEY,                    -- Auto-incrementing primary key
    userid       INT     NOT NULL,                      -- User owning the deck
    cardid       INT     NOT NULL,                      -- Card in the deck
    inbattledeck BOOLEAN NOT NULL,                      -- Whether the card is in the battle deck
    CONSTRAINT fk_user FOREIGN KEY (userid) REFERENCES users (id) ON DELETE CASCADE,
    CONSTRAINT fk_card FOREIGN KEY (cardid) REFERENCES cards (id) ON DELETE CASCADE,
    CONSTRAINT unique_user_card UNIQUE (userid, cardid) -- Prevent duplicate cards for a user
);

-- Battle table
CREATE TABLE battle
(
    id      SERIAL PRIMARY KEY,                         -- Auto-incrementing primary key
    player1 INT NOT NULL,                               -- Player 1 ID
    player2 INT NOT NULL,                               -- Player 2 ID
    winner  INT,                                        -- ID of the winning player (nullable if no winner yet)
    CONSTRAINT fk_player1 FOREIGN KEY (player1) REFERENCES users (id) ON DELETE CASCADE,
    CONSTRAINT fk_player2 FOREIGN KEY (player2) REFERENCES users (id) ON DELETE CASCADE,
    CONSTRAINT fk_winner FOREIGN KEY (winner) REFERENCES users (id) ON DELETE CASCADE,
    CONSTRAINT unique_players UNIQUE (player1, player2) -- Prevent duplicate battles between the same players
);

-- BattleLog table
CREATE TABLE battlelog
(
    id       SERIAL PRIMARY KEY,    -- Auto-incrementing primary key
    battleid INT          NOT NULL, -- Battle ID
    card1    INT          NOT NULL, -- Card 1 ID
    card2    INT          NOT NULL, -- Card 2 ID
    outcome  VARCHAR(255) NOT NULL, -- Outcome of the card battle (e.g., "Card1 wins")
    CONSTRAINT fk_battle FOREIGN KEY (battleid) REFERENCES battle (id) ON DELETE CASCADE,
    CONSTRAINT fk_card1 FOREIGN KEY (card1) REFERENCES cards (id) ON DELETE CASCADE,
    CONSTRAINT fk_card2 FOREIGN KEY (card2) REFERENCES cards (id) ON DELETE CASCADE
);
