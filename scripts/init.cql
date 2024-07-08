CREATE TABLE users (
    id uuid PRIMARY KEY,
    name text
);

CREATE TABLE wallets (
    id uuid,
    user_id uuid,
    balance decimal,
    PRIMARY KEY (id, user_id)
);

CREATE TABLE transactions (
    id uuid,
    from_wallet_id uuid,
    to_wallet_id uuid,
    amount decimal,
    nonce bigint,
    timestamp timestamp,
    from_wallet_balance_before decimal,
    from_wallet_balance_after decimal,
    to_wallet_balance_before decimal,
    to_wallet_balance_after decimal,
    PRIMARY KEY ((from_wallet_id, to_wallet_id), timestamp, id)
) WITH CLUSTERING ORDER BY (timestamp DESC);

CREATE TABLE transaction_counts (
    date date,
    count counter,
    PRIMARY KEY (date)
);

CREATE TABLE transaction_volumes (
    date date,
    volume counter,
    PRIMARY KEY (date)
);

CREATE TABLE active_users (
    date date,
    user_id uuid,
    PRIMARY KEY (date, user_id)
);