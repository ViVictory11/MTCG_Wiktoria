-- Check if the database exists and create it if not
DO
$$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = 'mydb') THEN
            PERFORM dblink_connect('dbname=postgres user=admin password=admin');
            PERFORM dblink_exec('CREATE DATABASE mydb');
            PERFORM dblink_disconnect();
        END IF;
    END;
$$;

-- Connect to the Database
\c mydb;

-- Create Users Table
CREATE TABLE IF NOT EXISTS users (
                                     id SERIAL PRIMARY KEY,
                                     username TEXT UNIQUE NOT NULL,
                                     password TEXT NOT NULL,
                                     coins INTEGER DEFAULT 20,
                                     elo INTEGER DEFAULT 100,
                                     games_played INTEGER DEFAULT 0,
                                     wins INTEGER DEFAULT 0,
                                     losses INTEGER DEFAULT 0,
                                     profile TEXT DEFAULT '',
                                     profile_image_url TEXT DEFAULT NULL,
                                     last_login TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                                     created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create Cards Table
CREATE TABLE IF NOT EXISTS cards (
                                     id SERIAL PRIMARY KEY,
                                     name TEXT UNIQUE NOT NULL,
                                     type TEXT NOT NULL CHECK (type IN ('Monster', 'Spell')),
                                     element TEXT NOT NULL CHECK (element IN ('Fire', 'Water', 'Normal')),
                                     damage INTEGER NOT NULL,
                                     rarity TEXT DEFAULT 'common' CHECK (rarity IN ('common', 'rare', 'epic', 'legendary')),
                                     special_ability TEXT DEFAULT NULL
);

-- Create User_Cards Table
CREATE TABLE IF NOT EXISTS user_cards (
                                          id SERIAL PRIMARY KEY,
                                          user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
                                          card_id INTEGER NOT NULL REFERENCES cards(id) ON DELETE CASCADE,
                                          in_deck BOOLEAN DEFAULT FALSE,
                                          locked_for_trade BOOLEAN DEFAULT FALSE,
                                          UNIQUE(user_id, card_id)
);

-- Create Decks Table
CREATE TABLE IF NOT EXISTS decks (
                                     id SERIAL PRIMARY KEY,
                                     user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
                                     card_id INTEGER NOT NULL REFERENCES cards(id) ON DELETE CASCADE,
                                     deck_name TEXT DEFAULT 'Default Deck',
                                     UNIQUE(user_id, card_id)
);

-- Create Trades Table
CREATE TABLE IF NOT EXISTS trades (
                                      id SERIAL PRIMARY KEY,
                                      user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
                                      offered_card_id INTEGER NOT NULL REFERENCES user_cards(id) ON DELETE CASCADE,
                                      requirement_type TEXT NOT NULL CHECK (requirement_type IN ('Monster', 'Spell')),
                                      min_damage INTEGER DEFAULT 0,
                                      status TEXT DEFAULT 'pending' CHECK (status IN ('pending', 'completed', 'canceled')),
                                      expires_at TIMESTAMP DEFAULT (CURRENT_TIMESTAMP + INTERVAL '7 days')
);

-- Create Battles Table
CREATE TABLE IF NOT EXISTS battles (
                                       id SERIAL PRIMARY KEY,
                                       user1_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
                                       user2_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
                                       winner_id INTEGER REFERENCES users(id),
                                       rounds_played INTEGER NOT NULL,
                                       created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create Battle_Log Table
CREATE TABLE IF NOT EXISTS battle_log (
                                          id SERIAL PRIMARY KEY,
                                          battle_id INTEGER NOT NULL REFERENCES battles(id) ON DELETE CASCADE,
                                          round_number INTEGER NOT NULL,
                                          user1_card_id INTEGER NOT NULL REFERENCES user_cards(id) ON DELETE CASCADE,
                                          user2_card_id INTEGER NOT NULL REFERENCES user_cards(id) ON DELETE CASCADE,
                                          winner_card_id INTEGER REFERENCES user_cards(id),
                                          user1_card_damage INTEGER,
                                          user2_card_damage INTEGER,
                                          log_details TEXT NOT NULL
);

-- Insert Sample Data into Users Table
INSERT INTO users (username, password, coins, profile) VALUES
                                                           ('player1', 'password123', 20, 'Player 1 profile info'),
                                                           ('player2', 'password456', 20, 'Player 2 profile info'),
                                                           ('player3', 'testpassword', 20, 'Player 3 profile info')
ON CONFLICT (username) DO NOTHING;

-- Insert Sample Data into Cards Table
INSERT INTO cards (name, type, element, damage, rarity, special_ability) VALUES
                                                                             ('Dragon', 'Monster', 'Fire', 100, 'epic', NULL),
                                                                             ('Goblin', 'Monster', 'Normal', 30, 'common', NULL),
                                                                             ('Water Blast', 'Spell', 'Water', 80, 'rare', NULL),
                                                                             ('Fire Ball', 'Spell', 'Fire', 70, 'common', NULL),
                                                                             ('Kraken', 'Monster', 'Water', 120, 'legendary', 'Immune to Spells'),
                                                                             ('Wizard', 'Monster', 'Normal', 60, 'rare', 'Controls Orks')
ON CONFLICT (name) DO NOTHING;

-- Insert Sample Data into User_Cards Table
INSERT INTO user_cards (user_id, card_id, in_deck) VALUES
                                                       (1, 1, TRUE), -- player1 owns Dragon and it's in the deck
                                                       (1, 3, TRUE), -- player1 owns Water Blast and it's in the deck
                                                       (1, 5, FALSE), -- player1 owns Kraken but not in the deck
                                                       (2, 2, TRUE), -- player2 owns Goblin and it's in the deck
                                                       (2, 4, TRUE), -- player2 owns Fire Ball and it's in the deck
                                                       (3, 6, FALSE) -- player3 owns Wizard but not in the deck
ON CONFLICT DO NOTHING;

-- Insert Sample Data into Trades Table
INSERT INTO trades (user_id, offered_card_id, requirement_type, min_damage, status) VALUES
                                                                                        (1, 5, 'Spell', 70, 'pending'),
                                                                                        (2, 2, 'Monster', 50, 'completed')
ON CONFLICT DO NOTHING;

-- Insert Sample Data into Battles Table
INSERT INTO battles (user1_id, user2_id, winner_id, rounds_played) VALUES
                                                                       (1, 2, 1, 3), -- player1 beat player2 in 3 rounds
                                                                       (2, 3, 2, 5) -- player2 beat player3 in 5 rounds
ON CONFLICT DO NOTHING;

-- Insert Sample Data into Battle_Log Table
INSERT INTO battle_log (battle_id, round_number, user1_card_id, user2_card_id, winner_card_id, user1_card_damage, user2_card_damage, log_details) VALUES
                                                                                                                                                      (1, 1, 1, 2, 1, 100, 30, 'Round 1: Dragon vs Goblin, Dragon wins with 100 damage.'),
                                                                                                                                                      (1, 2, 3, 4, 4, 80, 70, 'Round 2: Water Blast vs Fire Ball, Fire Ball wins with 70 damage.'),
                                                                                                                                                      (1, 3, 1, 2, 1, 100, 30, 'Round 3: Dragon vs Goblin, Dragon wins with 100 damage.'),
                                                                                                                                                      (2, 1, 4, 6, 4, 70, 60, 'Round 1: Fire Ball vs Wizard, Fire Ball wins with 70 damage.')
ON CONFLICT DO NOTHING;

-- Verify Tables
\d users
\d cards
\d user_cards
\d trades
\d battles
\d battle_log

-- Verify Data
SELECT * FROM users;
SELECT * FROM cards;
SELECT * FROM user_cards;
SELECT * FROM trades;
SELECT * FROM battles;
SELECT * FROM battle_log;
